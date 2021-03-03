<template>
	<div class="v-tab-option-alert">
		Настройки для подсчета кол-ва активностей в ОЭФ Клиенты
		<br />
		<br />Статус активности:
		<!-- v-bind (сокращается как ':'), v-on (сокращается как '@')-->
		<v-confirm-select :picklistName="'FBAlarmOptionActivityStatus'" :checkboxes="Statuses" @change="StatusConfirm"></v-confirm-select>Тип активности:
		<v-confirm-select :picklistName="'FBAlarmOptionActivityType'" :checkboxes="Types" @change="TypeConfirm"></v-confirm-select>
	</div>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Model } from 'vue-property-decorator';
import PageBase from '@/base-components/abstract/v-page-base';
import Getter from '@/decorators/Getter';
import TextInputComponent from './v-text-input.vue';
import ButtonTextInputButton from '@/model/client/ButtonTemplate';
import MenuComponent from '../../../base-components/v-menu.vue';
import ButtonTextInputComponent from '../../../base-components/v-button-text-input.vue';
import SelectComponent from './v-select.vue';
/*
export default - экспорт модуля только с классом OptionAlert для
доступа к компоненту v-tab-option-alert в других скриптах через любое имя
*/
@Component({ inheritAttrs: false })
export default class OptionAlert extends Vue {
	/*
	метод, который принимает в качестве входных данных строковое свойство состояния чекбоксов на клиенте 
	this.mutableValue, доступное для изменения и:
	1. записывает его значение в БД
	2. записывает его значение на клиенте
	*/
	StatusConfirm(value: string) {
		/*
		если входящее значение строкового свойства состояния чекбоксов на клиенте this.mutableValue, доступное для изменения, которое 
		содержится в параметре value такое же, как и соответствующее текущее значение this.Statuses на клиенте, то возврат из метода
		*/
		if (this.Statuses == value) return;
		/*
		создание AJAX метода сетевого запроса на сервер с помощью fetch, принимающего объект с 3-мя свойствами
		Category, Name и DefValue (значение чекбоксов из v-confirm-select)
		*/
		this.$api.createOptionDef({ Category: 'AlarmOption', Name: 'ActivityStatusClientMainViewCount', DefValue: value });
		/*
		записать входящее значение строкового свойства состояния чекбоксов на клиенте this.mutableValue, доступное для изменения,
		которое содержится в параметре value в значение чекбоксов списка 'FBAlarmOptionActivityStatus' на клиенте
		*/
		this.Statuses = value;
	}

	/*
	метод, который принимает в качестве входных данных строковое свойство состояния чекбоксов на клиенте 
	this.mutableValue, доступное для изменения и:
	1. записывает его значение в БД
	2. записывает его значение на клиенте
	*/
	TypeConfirm(value: string) {
		/*
		если входящее значение строкового свойства состояния чекбоксов на клиенте this.mutableValue, доступное для изменения, которое 
		содержится в параметре value такое же, как и соответствующее текущее значение this.Statuses на клиенте, то возврат из метода
		*/
		if (this.Types == value) return;
		/*
		создание AJAX метода сетевого запроса на сервер с помощью fetch, принимающего объект с 3-мя свойствами
		Category, Name и DefValue (значение чекбоксов из v-confirm-select)
		*/
		this.$api.createOptionDef({ Category: 'AlarmOption', Name: 'ActivityTypeClientMainViewCount', DefValue: value });
		/*
		записать входящее значение строкового свойства состояния чекбоксов на клиенте this.mutableValue, доступное для изменения,
		которое содержится в параметре value в значение чекбоксов списка 'FBAlarmOptionActivityType' на клиенте
		*/
		this.Types = value;
	}

	//--------------------------------------------------Statuses & Types is data of OptionAlert--------------------------------------------------
	Statuses: string | null = null; // текущее значение чекбоксов списка 'FBAlarmOptionActivityStatus' на клиенте
	Types: string | null = null; // текущее значение чекбоксов списка 'FBAlarmOptionActivityType' на клиенте
	//------------------------------------------------------------------------------------------------------------------------------------------

	// метод, выполняемый при создании компонента v-tab-option-alert загрузки значений чекбоксов из БД:
	async created() {
		/* 
		записать возвращаемое значение запроса на выбор значений чекбоксов из таблицы USEROPTIONDEF
		со значением колонки NAME='ActivityStatusClientMainViewCount' в постоянную queryStatus
		*/
		const queryStatus = await this.$api.select(
			"SELECT DEFVALUE FROM USEROPTIONDEF WHERE NAME='ActivityStatusClientMainViewCount'",
			'defValue',
		);
		/*
		если значение чекбоксов из таблицы USEROPTIONDEF БД есть, тогда в текущее значение 
		чекбоксов списка 'FBAlarmOptionActivityStatus' на клиенте записать значение из БД
		*/
		if (queryStatus != null && queryStatus.length != 0) {
			console.log('!!!' + queryStatus[0].defValue);
			this.Statuses = queryStatus[0].defValue;
		}

		/* 
		записать возвращаемое значение запроса на выбор значений чекбоксов из таблицы USEROPTIONDEF
		со значением колонки NAME='ActivityTypeClientMainViewCount' в постоянную queryType
		*/
		const queryType = await this.$api.select(
			"SELECT DEFVALUE FROM USEROPTIONDEF WHERE NAME='ActivityTypeClientMainViewCount'",
			'defValue',
		);
		/*
		если значение чекбоксов из таблицы USEROPTIONDEF БД есть, тогда в текущее значение 
		чекбоксов списка 'FBAlarmOptionActivityType' на клиенте записать значение из БД
		*/
		if (queryType != null && queryType.length != 0) {
			console.log('!!!' + queryType[0].defValue);
			this.Types = queryType[0].defValue;
		}
	}
}
</script>

<style lang="scss">
</style>