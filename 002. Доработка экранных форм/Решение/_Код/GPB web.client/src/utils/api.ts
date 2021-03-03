import { EntityId } from '@/model/interfaces/EntityBase';
import { LogType } from './../logger';
import sizeOfObject from './size-of-object';
import { DateTime } from 'luxon';
import logger from '../logger';

export type SDataConfig = {
	include?: string[] | undefined;
	where?:
	| { fieldName: string; operator: 'eq' | 'ne' | 'lt' | 'le' | 'gt' | 'ge' | 'like'; value: string | number }[]
	| undefined;
	select?: string[] | undefined;
};

export type ServerError = {
	applicationCode: string;
	message: string;
	payloadPath: string;
	sdataCode: string;
	severity: string;
	stackTrace: string;
};

export default {
	get basePath(): string {
		return '/SlxClient/QueryService.aspx';
	},
	get baseSDataPath(): string {
		return '/SlxClient/slxdata.ashx/slx/dynamic/-';
	},
	get baseSDataServicePath(): string {
		return '/SlxClient/slxdata.ashx/slx/crm/-';
	},
	get baseSDataSystemPath(): string {
		return '/SlxClient/slxdata.ashx/slx/system/-';
	},
	get intRouter(): string {
		return '/SlxClient/QueryService.aspx';
	},
	async select(query: string, properties?: string, logType: LogType = 'query'): Promise<Array<any> | never> {
		let select = query.trim();
		if (select.endsWith(';')) select = select.substring(0, select.length - 1);

		const headers = new Headers({
			query: btoa(encodeURIComponent(select)),
		});
		if (properties) headers.append('properties', btoa(encodeURIComponent(properties)));
		if (process.env.NODE_ENV === 'development') var time = performance.now(); // eslint-disable-line no-var
		const result = await fetch(this.basePath + '?' + new Date().getTime(), {
			method: 'GET',
			headers: headers,
			credentials: 'include',
			cache: 'default',
		}).then(x => (x.ok ? x.json() : x.text()));
		if (process.env.NODE_ENV === 'development') {
			logger.log(logType, {
				select: select,
				result: Array.isArray(result) ? result.slice(0) : result,
				size: sizeOfObject(result),
				time: performance.now() - time!, // eslint-disable-line @typescript-eslint/no-unnecessary-type-assertion
			});
		}
		if (!Array.isArray(result)) throw result;

		return result;
	},
	async multiSelect(
		queries: { key: string; query: string; properties?: string }[],
		logType: LogType = 'query',
	): Promise<{ [key: string]: Array<Array<any> | any> | ServerError }> {
		if (process.env.NODE_ENV === 'development') var time = performance.now(); // eslint-disable-line no-var
		const headers = new Headers({
			method: 'MultiSelect',
			queries: JSON.stringify(
				queries.map(x => ({
					query: btoa(encodeURIComponent(x.query)),
					properties: x.properties ? btoa(encodeURIComponent(x.properties)) : null,
					key: x.key,
				})),
			),
		});
		const result: { [key: string]: Array<Array<any> | any> | ServerError } = await fetch(
			this.basePath + '?' + new Date().getTime(),
			{
				method: 'GET',
				headers: headers,
				credentials: 'include',
				cache: 'default',
			},
		).then(x => (x.ok ? x.json() : x.text()));
		if (process.env.NODE_ENV === 'development') {
			time = performance.now() - time!; // eslint-disable-line @typescript-eslint/no-unnecessary-type-assertion
			logger.log(
				logType,
				queries.map(x => ({ ...x, result: result[x.key], size: sizeOfObject(result[x.key]), time })),
			);
		}
		return result;
	},
	template(type: string): Promise<object | [ServerError]> {
		return fetch(`${this.baseSDataPath}/${type}/$template?_compact=true&format=json&_t=${Date.now()}`, {
			credentials: 'include',
			headers: { 'content-type': 'application/json' },
			method: 'GET',
			mode: 'cors',
		}).then(x => x.json());
	},
	create(type: string, entity?: object, config: SDataConfig = {}, logType: LogType = 'query'): Promise<Response> {
		let options = '';
		if (config.where)
			options += '&include=' + config.where.map(x => `${x.fieldName} ${x.operator} ${x.value}`).join(' and ');
		if (config.include) options += '&include=' + config.include.join(',');
		if (config.select) options += '&select=' + config.select.join(',');
		if (process.env.NODE_ENV === 'development') logger.log(logType, `create entity ${type} `, entity);

		return fetch(`${this.baseSDataPath}/${type}?_compact=true&format=json&_t=${Date.now()}${options}`, {
			credentials: 'include',
			headers: { 'content-type': 'application/json' },
			body: JSON.stringify(entity || {}),
			method: 'POST',
			mode: 'cors',
		});
	},
	read(type: string, key: string, config: SDataConfig = {}, logType: LogType = 'query') {
		let options = '';
		if (config.where)
			options += '&include=' + config.where.map(x => `${x.fieldName} ${x.operator} ${x.value}`).join(' and ');
		if (config.include) options += '&include=' + config.include.join(',');
		if (config.select) options += '&select=' + config.select.join(',');
		if (process.env.NODE_ENV === 'development') {
			logger.log(logType, `read entity ${type}`, key);
			var time = performance.now(); // eslint-disable-line no-var
		}
		return fetch(`${this.baseSDataPath}/${type}('${key}')?_compact=true&format=json&_t=${Date.now()}${options}`, {
			credentials: 'include',
			headers: { 'content-type': 'application/json' },
			method: 'Get',
			mode: 'cors',
		}).then(x => {
			if (process.env.NODE_ENV === 'development') logger.log(logType, 'read ended', key, performance.now() - time);
			return x;
		});
	},
	update(
		type: string,
		key: string,
		entity: object,
		config: SDataConfig = {},
		logType: LogType = 'query',
	): Promise<Response> {
		let options = '';
		if (config.where)
			options += '&include=' + config.where.map(x => `${x.fieldName} ${x.operator} ${x.value}`).join(' and ');
		if (config.include) options += '&include=' + config.include.join(',');
		if (config.select) options += '&select=' + config.select.join(',');
		if (process.env.NODE_ENV === 'development') logger.log(logType, `update entity ${type}`, key, entity);
		return fetch(`${this.baseSDataPath}/${type}('${key}')?_compact=true&format=json&_t=${Date.now()}${options}`, {
			credentials: 'include',
			headers: { 'content-type': 'application/json' },
			body: JSON.stringify(entity),
			method: 'PUT',
			mode: 'cors',
		});
	},
	delete(type: string, key: string, logType: LogType = 'query'): Promise<Response> {
		if (process.env.NODE_ENV === 'development') logger.log(logType, `delete entity ${type}`, key);
		return fetch(`${this.baseSDataPath}/${type}('${key}')?_compact=true&format=json&_t=${Date.now()}`, {
			credentials: 'include',
			method: 'DELETE',
			mode: 'cors',
		});
	},
	deleteActivity(key: string): Promise<Response> {
		if (process.env.NODE_ENV === 'development') logger.log('query', `delete activity`, key);
		return fetch(this.basePath + '?' + new Date().getTime(), {
			method: 'GET',
			headers: new Headers({
				method: 'deleteActivities',
				payload: key,
			}),
			credentials: 'include',
			cache: 'default',
		});
	},

	/* 
	создание AJAX метода сетевого запроса на сервер с помощью fetch, принимающего объект с 3-мя свойствами
	Category, Name и DefValue (значение чекбоксов из v-confirm-select), возвращаемым значением которого 
	является interface Promise<T> - завершение асинхронной операции с параметром Response - этот интерфейс 
	Fetch API представляет ответ на запрос. 
	*/
	createOptionDef(payload: { Category: string; Name: string; DefValue: string }): Promise<Response> {
		if (process.env.NODE_ENV === 'development') logger.log('query', `В методе createOptionDef()`);	// лог

		// формирование запроса с параметрами: путь '/SlxClient/QueryService.aspx' и время
		return fetch(this.basePath + '?' + new Date().getTime(), {
			method: 'GET',																				// методом GET
			headers: new Headers({																		// заголовки
				/*
				метод обработки AJAX запроса с клиента и формирования сессии записи в БД 
				принятых данных, расположенный в файле '/SlxClient/QueryService.aspx'
				*/
				method: 'CreateUserOptionDef',

				/*
				Параметры, заданные в ТЗ, обрабатываются мтодом encodeURIComponent(), который кодирует
				текстовую строку как допустимый компонент универсального идентификатора ресурса (URI),
				а метод btoa() кодирует строку в ACSII строку base-64, которая будет представлена с 
				помощью 64 символов: "A-Z", "a-z", "0-9", "+", "/" и "="
				*/
				Category: btoa(encodeURIComponent(payload.Category)),									// ТЗ
				Name: btoa(encodeURIComponent(payload.Name)),

				// значение чекбоксов из v-confirm-select
				DefValue: btoa(encodeURIComponent(payload.DefValue)),
			}),
			credentials: 'include',
			cache: 'default',
		});
	},
	completeActivity(entity: any, userId: EntityId, logType: LogType = 'query'): Promise<Response> {
		if (process.env.NODE_ENV === 'development') logger.log(logType, `complete activity`, entity, userId);
		return fetch(
			`${this.baseSDataSystemPath}/activities/%24service/Complete?_compact=true&format=json&_t=${Date.now()}`,
			{
				credentials: 'include',
				headers: {
					'content-type': 'application/json',
				},
				body: JSON.stringify({
					$name: 'Complete',
					request: {
						entity,
						userId,
						result: 'Завершена',
						completeDate: DateTime.utc(),
					},
				}),
				method: 'POST',
				mode: 'cors',
			},
		);
	},
	deleteCompletedSteps(opportunityId: string, logType: LogType = 'query'): Promise<Response> {
		if (process.env.NODE_ENV === 'development') logger.log(logType, `delete completed steps`, opportunityId);
		return fetch(this.basePath + '?' + new Date().getTime(), {
			method: 'GET',
			headers: new Headers({
				method: 'deleteCompletedSteps',
				payload: opportunityId,
			}),
			credentials: 'include',
			cache: 'default',
		});
	},
	deleteOfferActivities(offerId: string, logType: LogType = 'query'): Promise<Response> {
		if (process.env.NODE_ENV === 'development') logger.log(logType, `delete offer activities`, offerId);
		return fetch(this.basePath + '?' + new Date().getTime(), {
			method: 'GET',
			headers: new Headers({
				method: 'deleteOfferActivities',
				payload: offerId,
			}),
			credentials: 'include',
			cache: 'default',
		});
	},
	async getPicklist(name: string, enableCache = false, logType: LogType = 'query'): Promise<any> {
		let picklist = enableCache ? window.sessionStorage.getItem(`fb_picklist_${name}`) : null;
		if (!picklist) {
			picklist = await fetch(this.basePath + '?' + new Date().getTime(), {
				method: 'GET',
				headers: new Headers({
					method: 'picklist',
					payload: btoa(encodeURIComponent(name)),
				}),
				credentials: 'include',
				cache: 'default',
			}).then(x => (x.ok ? x.json() : x.text()));
			if (typeof picklist === 'object') {
				if (process.env.NODE_ENV === 'development') logger.log(logType, `get picklist '${name}'`);
				window.sessionStorage.setItem(`fb_picklist_${name}`, JSON.stringify(picklist));
			}
		} else {
			picklist = JSON.parse(picklist);
		}
		return picklist;
	},
	async getEntityHistory(
		logType: LogType = 'query',
	): Promise<{ Description: string; EntityId: string; EntityType: string }[]> {
		if (process.env.NODE_ENV === 'development') logger.log(logType, `get entity history`);
		return await fetch(this.basePath + '?' + new Date().getTime(), {
			method: 'GET',
			headers: new Headers({
				method: 'historyService',
			}),
			credentials: 'include',
			cache: 'default',
		}).then(x => (x.ok ? x.json() : x.text()));
	},
	readSystem(type: string, key: string, config: SDataConfig = {}, logType: LogType = 'query') {
		let options = '';
		if (config.where)
			options += '&include=' + config.where.map(x => `${x.fieldName} ${x.operator} ${x.value}`).join(' and ');
		if (config.include) options += '&include=' + config.include.join(',');
		if (config.select) options += '&select=' + config.select.join(',');
		if (process.env.NODE_ENV === 'development') {
			logger.log(logType, `read entity ${type}`, key);
			var time = performance.now(); // eslint-disable-line no-var
		}
		return fetch(`${this.baseSDataSystemPath}/${type}('${key}')?_compact=true&format=json&_t=${Date.now()}${options}`, {
			credentials: 'include',
			headers: { 'content-type': 'application/json' },
			method: 'Get',
			mode: 'cors',
		}).then(x => {
			if (process.env.NODE_ENV === 'development') logger.log(logType, 'read ended', key, performance.now() - time);
			return x;
		});
	},
	updateSystem(type: string, key: string, entity: object, config: SDataConfig = {}): Promise<Response> {
		let options = '';
		if (config.where)
			options += '&include=' + config.where.map(x => `${x.fieldName} ${x.operator} ${x.value}`).join(' and ');
		if (config.include) options += '&include=' + config.include.join(',');
		if (config.select) options += '&select=' + config.select.join(',');
		process.env.NODE_ENV === 'development' && logger.log('query', `update entity ${type}`, key, entity);
		return fetch(`${this.baseSDataSystemPath}/${type}('${key}')?_compact=true&format=json&_t=${Date.now()}${options}`, {
			credentials: 'include',
			headers: { 'content-type': 'application/json' },
			body: JSON.stringify(entity),
			method: 'PUT',
			mode: 'cors',
		});
	},
	deleteSystem(type: string, key: string): Promise<Response> {
		// var request = new Sage.SData.Client.SDataSingleResourceRequest(Sage.Data.SDataServiceRegistry.getSDataService('dynamic'));
		// request.setResourceKind(type);
		// request.setResourceSelector(`'${key}'`);
		// request.delete({$key:key});
		process.env.NODE_ENV === 'development' && logger.log('query', `delete entity ${type}`, key);
		return fetch(`${this.baseSDataSystemPath}/${type}('${key}')?_compact=true&format=json&_t=${Date.now()}`, {
			credentials: 'include',
			method: 'DELETE',
			mode: 'cors',
		});
	},

	clearCurrentGroupCache(): Promise<Response> {
		process.env.NODE_ENV === 'development' && logger.log('query', `clearCurrentGroupCache`);
		return fetch(this.basePath + '?' + new Date().getTime(), {
			method: 'GET',
			headers: new Headers({
				method: 'clearCurrentGroupCache',
			}),
			credentials: 'include',
			cache: 'default',
		});
	},

	RunPackageCommon(params: Record<string, any>) {
		return fetch(`${this.intRouter}`, {
			method: 'POST',
			cache: 'no-cache',
			headers: new Headers({
				method: 'RunPackage',
				'Content-Type': 'application/json',
			}),
			body: JSON.stringify(params || {}),
			credentials: 'include',
			mode: 'cors',
		});
	},

	logger(type: string, entityId: EntityId, entityType: string, description: string, isTab: boolean) {
		return fetch(this.basePath + '?' + new Date().getTime(), {
			method: 'GET',
			headers: new Headers({
				method: 'FbLogger',
				type: btoa(encodeURIComponent(type)),
				entityId: btoa(encodeURIComponent(entityId)),
				entityType: btoa(encodeURIComponent(entityType)),
				description: btoa(encodeURIComponent(description)),
				isTab: btoa(encodeURIComponent(isTab ? 'true' : 'false')),
			}),
			credentials: 'include',
			cache: 'default',
		});
	},

	/**
	 * Вызывает метод QueryService
	 * @param method название метода. Необходимо определить в serviceTypesMap вместе с типом параметра cfg
	 * @param cfg объект с параметрами методов
	 */
	service<T extends keyof typeof serviceTypesMap>(method: T, cfg: typeof serviceTypesMap[T]) {
		const headers: { [key: string]: string } = {};
		for (const [name, value] of Object.entries(cfg)) headers[name] = btoa(encodeURIComponent(value));
		headers['method'] = method;
		process.env.NODE_ENV === 'development' && logger.log('service', method, cfg);
		return fetch(this.basePath + '?' + new Date().getTime(), {
			method: 'GET',
			headers: new Headers(headers),
			credentials: 'include',
			cache: 'default',
		});
	},

	/**
	 * Вызывает метод бизнес-правил через SData
	 * @param type путь сущности SData. Надо определить в entityBuisnessRuleMethodMap
	 * @param methodName метод бинес-правил. Надо определить через "|"" в entityBuisnessRuleMethodMap у соответствующего type
	 * @param request Параметры, которые принимает метод можно посмотреть через 
	 * await fetch(`/SlxClient/slxdata.ashx/slx/dynamic/-/opportunities/$service/CalculateSalesPotential/$template?_compact=true&format=json&_t=${Date.now()}`, {
			credentials: 'include',
			headers: {
				'content-type': 'application/json',
			},
			method: 'GET',
			mode: 'cors',
		}).then(x=>x.json()); 
		Надо определить в buisnessRuleMethodTypesMap по аналогии.
	 */
	buisnessRuleMethod<
		E extends keyof typeof entityBuisnessRuleMethodMap,
		D extends keyof typeof buisnessRuleMethodTypesMap,
		T extends typeof entityBuisnessRuleMethodMap[E],
		S extends typeof buisnessRuleMethodTypesMap[D]
	>(type: E, methodName: T, request: S['request']): Promise<S['responce'] | [ServerError]> {
		process.env.NODE_ENV === 'development' && logger.log('service', type, methodName, request);
		return fetch(
			`${this.baseSDataPath}/${type}/$service/${methodName}?_compact=true&format=json&t=${new Date().getTime()}`,
			{
				method: 'POST',
				headers: new Headers({ 'content-type': 'application/json' }),
				credentials: 'include',
				cache: 'default',
				body: JSON.stringify({ request }),
			},
		)
			.then(x => x.json())
			.then(x => (Array.isArray(x) ? x : x.response.Result));
	},
};

const serviceTypesMap = {
	urlEncode1251: {} as { payload: string },
	FbLogger: {} as {
		type: string;
		entityId: EntityId;
		entityType: string;
		description: string;
		isTab: 'true' | 'false';
	},
	ValidateWorkerRole: {} as { entityId: EntityId },
	StateRegionPrimeChanged: {} as { entityId: EntityId; entity: 'state' | 'regionPrime' },
	RegionChanged: {} as { entityId: EntityId; regionId: EntityId },
	PrognozGreenDataChanged: {} as { entityId: EntityId; mode: 'change' | 'clear'; entity: 'prognoz' | 'greenData' },
	ValidateInnOkpo: {} as { entityId: EntityId; code: string; mode: 'inn' | 'okpo' },
	StatusChanged: {} as { entityId: EntityId; newStatus: string },
	CompaniesGroupChanged: {} as { entityId: EntityId; companiesGroupId: string; mode: 'change' | 'clear' },
	DivisionChanged: {} as { entityId: EntityId; divisionId: EntityId; mode: 'change' | 'clear' },
	SupportManagerChanged: {} as { entityId: EntityId; workerRoleId: EntityId; mode: 'change' | 'clear' },
	ClientManagerChanged: {} as { entityId: EntityId; workerRoleId: EntityId; mode: 'change' | 'clear' },
	createAuditRecord: {} as {
		entity: string;
		entityId: EntityId;
		fieldName: string;
		displayName: string;
		oldValue: string;
		newValue: string;
		isId: '1' | '0';
		userId: EntityId;
		description: string;
	},
	AddPicklistItem: {} as { Text: string; ShortText: string; PickListId: EntityId; LanguageCode: string },
	AccountAddRevenue: {} as { entityId: EntityId; revenue: number; year: string; mode: 'change' | 'clear' },
};

/**
 * TODO Объеденить объекты buisnessRuleMethodTypesMap и entityBuisnessRuleMethodMap в один {opportunitites: {getEstimatedClose:{...}}}
 * К сожалению TS ругается на typeof buisnessRuleMethodTypesMap[T][keyof T] и в доках я примера не нашел
 */
const buisnessRuleMethodTypesMap = {
	getEstimatedClose: {
		request: {} as {}, //Тут используется as так как нужен только тип
		responce: {} as string,
	},
	IsMultiCurrencyEnabled: {
		request: {} as {},
		responce: {} as boolean,
	},
};
const entityBuisnessRuleMethodMap = {
	opportunities: {} as 'getEstimatedClose' | 'IsMultiCurrencyEnabled',
};
