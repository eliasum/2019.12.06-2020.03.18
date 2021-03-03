// Account.store.ts - глобальное хранилище данных для страниц клиента

import {
	IAccFilial,
	toServerView as accFilialtoServerView,
	toClientView as accFilialToClientView,
} from './../model/interfaces/IAccFilial';
import {
	IAddress,
	toServerView as addressToServerView,
	toClientViewFromSelect as addressToClientViewFromSelect,
	toClientView as addressToClientView,
} from '@/model/interfaces/IAddress';
import {
	IACCOUNTASSISTANT,
	toServerView as accountAssistantToServerView,
	toClientViewFromSelect as accountAssistantToClientViewFromSelect,
	toClientView as accountAssistantToClientView,
} from '@/model/interfaces/IAccountAssistant';
import { GetterTree, ActionTree, MutationTree, Module } from 'vuex';
import {
	IAccountExt,
	toClientViewAccountExt as accountExtToClientView,
	toServerViewAccountExt as accountExtToServerView,
} from '@/model/interfaces/IAccountExt';
import {
	IAccount,
	toClientView as accountToClientView,
	toServerView as accountToServerView,
} from '@/model/interfaces/IAccount';
import { RootState } from '.';
import { EntityId } from '@/model/interfaces/EntityBase';
import TabBase from '@/base-components/abstract/v-tab-base';
import api from '@/utils/api';
import { dateTimeFromSDataDate } from '@/utils/helpers';
import { DateTime } from 'luxon';
import { escape } from 'sqlstring';
import Helper from '@/model/client/Helpers';
import Contact from '@/model/client/Contact';
import {
	IAccContactInfo,
	toServerView as contactToServerView,
	toClientView as contactToClientView,
	toClientViewFromSelect as contactToClientViewFromSelect,
} from '@/model/interfaces/IAccContactInfo';
import { toServerView as communicationToServerView } from '@/model/interfaces/IAccNewsletter';
import Address from '@/model/client/Address';
import Filial, { ToClientView as filialToClientView, queryFilials } from '@/model/client/Filial';

export type AccountExtended = {
	account: IAccount;
	accountExtension: IAccountExt;
	email: IAccContactInfo | null;
	address: IAddress | null;
	officialPhone: IAccContactInfo | null;
	privatePhone: IAccContactInfo | null;
};

export const namespaced = true;
export const state: {
	_entity: AccountExtended | null;
	_tabs: TabBase[];
	_helpers: IACCOUNTASSISTANT[];
	_contacts: Contact[];
	_addresses: Address[];
	_contactsTabLoaded: boolean;
	_filials: Filial[];
	_filialsTabLoaded: boolean;
} = {
	_entity: null,
	_tabs: [],
	_addresses: [],
	_helpers: [],
	_contacts: [],
	_contactsTabLoaded: false,
	_filials: [],
	_filialsTabLoaded: false,
};

// доступ к состоянию
export const getters: GetterTree<typeof state, RootState> = {
	entity: state => state._entity,
	getTabByTabId: state => (tabId: string) => {
		for (const tab of state._tabs) {
			if (tab.tabId === tabId) {
				return tab;
			}
		}
		return null;
	},
	helpers: state => state._helpers,
	contacts: state => state._contacts,
	addresses: state => state._addresses,
	contactsTabLoaded: state => state._contactsTabLoaded,
	filials: state => state._filials,
	filialsTabLoaded: state => state._filialsTabLoaded,
};

// отрабатывает реактивность Vue (изменяет состояние стора)
export const mutations: MutationTree<typeof state> = {
	loadData(state, payload: AccountExtended) {
		state._entity = payload;
	},
	AddTab(state, payload: TabBase) {
		state._tabs.push(payload);
	},
	Clear(state) {
		state._entity = null;
		state._tabs = [];
		state._addresses = [];
		state._contacts = [];
		state._helpers = [];
	},
	CommentaryUpdate(state, payload: string) {
		state._entity!.accountExtension.ClientComments = payload;
	},
	NotesUpdate(state, payload: string) {
		state._entity!.accountExtension.ClientNotes = payload;
	},
	helperMainUpdate(state, payload: string) {
		state._entity!.accountExtension.IsThroughAssistantComm = payload;
	},
	LoadHelpers(state, payload: IACCOUNTASSISTANT[]) {
		state._helpers = payload;
	},
	LoadContacts(state, payload: Contact[]) {
		state._contacts = payload;
	},
	LoadAddresses(state, payload: Address[]) {
		state._addresses = payload;
	},
	LoadFilials(state, payload: Filial[]) {
		state._filials = payload;
	},
	AddHelper(state, payload: IACCOUNTASSISTANT) {
		if (payload.ISMAIN) {
			for (const helper of state._helpers) helper.ISMAIN = false;
		}
		state._helpers.push(payload);
	},
	RemoveHelper(state, payload: EntityId) {
		for (let i = 0; i < state._helpers.length; i++) {
			if (state._helpers[i].$key == payload) state._helpers.splice(i, 1);
		}
	},
	EnsureContactPrimary(state, payload: Contact) {
		if (payload.contactType && payload.contactType.includes('Тел')) {
			for (const contact of state._contacts) {
				if (
					contact.$key != payload.$key &&
					contact.contactType &&
					contact.contactType.includes('Тел') &&
					contact.isPrimary
				)
					contact.isPrimary = false;
			}
		}
		if (
			payload.contactType &&
			(payload.contactType.toLowerCase().includes('e-mail') || payload.contactType.toLowerCase().includes('email'))
		) {
			for (const contact of state._contacts) {
				if (
					contact.$key != payload.$key &&
					contact.contactType &&
					(payload.contactType.toLowerCase().includes('e-mail') ||
						payload.contactType.toLowerCase().includes('email')) &&
					contact.isPrimary
				)
					contact.isPrimary = false;
			}
		}
	},
	AddContact(state, payload: Contact) {
		state._contacts.push(payload);
	},
	RemoveContact(state, payload: EntityId) {
		for (let i = 0; i < state._contacts.length; i++) {
			if (state._contacts[i].$key == payload) state._contacts.splice(i, 1);
		}
	},
	FinalizeContactsTabLoading(state) {
		state._contactsTabLoaded = true;
	},
	FinalizeFilialsTabLoading(state) {
		state._filialsTabLoaded = true;
	},
	SetAllHelpersExist(state) {
		for (const helper of state._helpers) helper.$state = 'exist';
	},
	SetAllContactsExist(state) {
		for (const contact of state._contacts) contact.$state = 'exist';
	},
	SetAllAddressesExist(state) {
		for (const address of state._addresses) address.$state = 'exist';
	},
	AddAddress(state, payload: Address) {
		if (payload.primary || payload.mailing) {
			for (const address of state._addresses) {
				if (address.primary && payload.primary) address.primary = false;
				if (address.mailing && payload.mailing) address.mailing = false;
			}
		}
		state._addresses.push(payload);
	},
	RemoveAddress(state, payload: EntityId) {
		for (let i = 0; i < state._addresses.length; i++) {
			if (state._addresses[i].$key == payload) state._addresses.splice(i, 1);
		}
	},
	UpdateEmail(state, payload: IAccContactInfo) {
		if (!state._entity) return;
		state._entity.email = payload;
	},
	UpdateOfficialPhone(state, payload: IAccContactInfo) {
		if (!state._entity) return;
		state._entity.officialPhone = payload;
	},
	UpdatePrivatePhone(state, payload: IAccContactInfo) {
		if (!state._entity) return;
		state._entity.privatePhone = payload;
	},
	AddFilial(state, payload: Filial) {
		state._filials.push(payload);
	},
	UpdateFilial(state, payload: Filial) {
		if (!state._entity) return;
		const updatedFilial = state._filials.find(x => x.$key === payload.$key);
		if (updatedFilial) {
			const index = state._filials.indexOf(updatedFilial);
			state._filials[index] = payload;
		}
	},
	RemoveFilial(state, payload: EntityId) {
		for (let i = 0; i < state._filials.length; i++) {
			if (state._filials[i].$key == payload) state._filials.splice(i, 1);
		}
	},
	changeAddress(state, payload: IAddress) {
		if (state._entity) state._entity.address = payload;
	}
};

// действия - то что вызывается снаружи стора (вызывают мутации, но не изменяет состояние стора)
export const actions: ActionTree<typeof state, RootState> = {
	async load({ commit }, key: EntityId): Promise<boolean> {
		if (!key) return false;

		const account: IAccount = await api.read('Accounts', key).then(x => x.json());
		if (!account || (Array.isArray(account) && 'message' in account[0])) return false;

		const accountExt: IAccountExt = await api.read('accountExts', key).then(x => x.json());
		if (!accountExt || (Array.isArray(accountExt) && 'message' in accountExt[0])) return false;

		let primaryEmail: IAccContactInfo | null = null;
		if (accountExt.EmailId) primaryEmail = await api.read('accContactInfoes', accountExt.EmailId).then(x => x.json());

		let officialPhone: IAccContactInfo | null = null;
		if (accountExt.OfficialPhoneId)
			officialPhone = await api.read('accContactInfoes', accountExt.OfficialPhoneId).then(x => x.json());

		let privatePhone: IAccContactInfo | null = null;
		if (accountExt.PrivatePhoneId)
			privatePhone = await api.read('accContactInfoes', accountExt.PrivatePhoneId).then(x => x.json());

		let address: IAddress | null = null; // по умолчанию нет адреса
		/*
		Если accountExt.AddressID не null, то записать в address значение accountExt.AddressID сущности fbContactInfoes
		из SData в формате json
		*/
		if (accountExt.AddressID) address = await api.read('addresses', accountExt.AddressID).then(x => x.json());

		const entity: AccountExtended = {
			account: accountToClientView(account, 'exist'),
			accountExtension: accountExtToClientView(accountExt, 'exist'),
			email: primaryEmail ? contactToClientView(primaryEmail, 'exist') : null,
			privatePhone: privatePhone ? contactToClientView(privatePhone, 'exist') : null,
			officialPhone: officialPhone ? contactToClientView(officialPhone, 'exist') : null,
			address: address ? addressToClientView(address, 'exist') : null,
		};

		commit('loadData', entity);
		return true;
	},
	addTab({ commit }, tab: TabBase) {
		commit('AddTab', tab);
	},
	async showTab({ getters }, tabId: string) {
		const tab: TabBase | null = getters.getTabByTabId(tabId);

		if (tab) tab.show();

		while (!window.TabControl) await new Promise(resolve => setTimeout(resolve, 100));

		if (window.TabControl.getState().isMainTab(tabId)) {
			window.TabControl.showMainTab(tabId);
		} else if (window.TabControl.getState().isMoreTab(tabId)) {
			if (window.TabControl.getState().getActiveMoreTab() != 'More') {
				window.TabControl.showMainTab('More');
			}
			window.TabControl.showMoreTab(tabId);
		}
	},
	async save({ state }): Promise<boolean> {
		if (!state._entity || !state._entity.accountExtension) return false;

		let response = null;
		try {
			if (state._entity.accountExtension.$key) {
				response = await api
					.update(
						'accountExts',
						state._entity.accountExtension.$key,
						accountExtToServerView(state._entity.accountExtension),
					)
					.then(x => x.json());
			}
		} catch {
			return false;
		}

		if (!response || 'message' in response) return false;
		return true;
	},
	async delete({ commit }, payload: EntityId): Promise<boolean> {
		if (!payload) return false;

		let response = null;
		try {
			response = await api.delete('accounts', payload);
		} catch {
			return false;
		}
		if (!response || !response.ok) return false;
		commit('Clear');
		return true;
	},
	commentaryUpdate({ commit, state }, payload: string): boolean {
		if (!state._entity || !state._entity.accountExtension) return false;
		commit('CommentaryUpdate', payload);
		return true;
	},
	notesUpdate({ commit }, payload: string): boolean {
		if (!state._entity || !state._entity.accountExtension) return false;
		commit('NotesUpdate', payload);
		return true;
	},
	helperMainUpdate({ commit }, payload: boolean): boolean {
		if (!state._entity || !state._entity.accountExtension) return false;
		commit('helperMainUpdate', payload ? 'T' : 'F');
		return true;
	},
	async loadHelpersContacts({ commit }, key: EntityId): Promise<boolean> {
		if (!key) return false;

		let query = api.select(
			`select ACCOUNT_ASSISTANTID, FULL_NAME, ISMAIN, COMPANYNAME, DIVISION, TITLE, NOTES 
				from ACCOUNT_ASSISTANT where ACCOUNTID = ${escape(key)}`,
			'$key, FULLNAME, ISMAIN, COMPANYNAME, DIVISION, TITLE, Notes',
		);

		let innerQuery = null;

		const queryList: any[] = await query;
		const accountAssistantsList: IACCOUNTASSISTANT[] = [];
		if (queryList && queryList[0]) {
			for (const queryObject of queryList) {
				const phoneQueryString = {
					key: 'phone',
					query: `select ACC_CONTACT_INFOID, CONTACT_TYPE, CONTACT_VALUE 
							from ACC_CONTACT_INFO
							where ACCOUNT_ASSISTANTID = ${escape(queryObject.$key)} 
								and IS_MAIN = 'T' 
								and CONTACT_TYPE like '%Тел%'`,
					properties: '$key, ContactType, ContactValue',
				};
				const emailQueryString = {
					key: 'email',
					query: `select ACC_CONTACT_INFOID, CONTACT_TYPE, CONTACT_VALUE 
							from ACC_CONTACT_INFO
							where ACCOUNT_ASSISTANTID = ${escape(queryObject.$key)} 
								and IS_MAIN = 'T' 
								and CONTACT_TYPE like '%E-mail%'`,
					properties: '$key, ContactType, ContactValue',
				};
				const addressQueryString = {
					key: 'address',
					query: `select ADDRESSID, DESCRIPTION, FULL_ADDRESS
										from ADDRESS
										where ENTITYID = ${escape(queryObject.$key)}
											and ISPRIMARY = 'T'`,
					properties: '$key, Description, FullAddress',
				};
				innerQuery = api.multiSelect([phoneQueryString, emailQueryString, addressQueryString]);
				const { phone, email, address } = (await innerQuery) as { phone: any[]; email: any[]; address: any[] };

				const accountAssistant = accountAssistantToClientViewFromSelect(queryObject, 'exist');
				if (phone && phone[0]) accountAssistant.$primaryPhone = contactToClientViewFromSelect(phone[0], 'exist');
				if (email && email[0]) accountAssistant.$primaryEmail = contactToClientViewFromSelect(email[0], 'exist');
				if (address && address[0])
					accountAssistant.$primaryAddress = addressToClientViewFromSelect(address[0], 'exist');
				accountAssistantsList.push(accountAssistant);
			}
			commit('LoadHelpers', accountAssistantsList);
		}

		query = api.select(
			`select ACC_CONTACT_INFOID, CONTACT_TYPE, CONTACT_VALUE, IS_MAIN, 
			(select SUBSCRIPTION from ACC_NEWSLETTER 
				where DELIVERY_TYPE = 'Маркетинг' 
				and ACC_CONTACT_INFOID = ACC_CONTACT_INFO.ACC_CONTACT_INFOID)
					as marketing,
			(select SUBSCRIPTION from ACC_NEWSLETTER 
				where DELIVERY_TYPE = 'Инф. рассылка' 
				and ACC_CONTACT_INFOID = ACC_CONTACT_INFO.ACC_CONTACT_INFOID)
					as newsletter,
			(select SUBSCRIPTION from ACC_NEWSLETTER 
				where DELIVERY_TYPE = 'Конфиденц. инф-ция' 
				and ACC_CONTACT_INFOID = ACC_CONTACT_INFO.ACC_CONTACT_INFOID)
					as confidential,
			(select SUBSCRIPTION from ACC_NEWSLETTER 
				where DELIVERY_TYPE = 'ПФК' 
				and ACC_CONTACT_INFOID = ACC_CONTACT_INFO.ACC_CONTACT_INFOID)
					as PFK,
			(select SUBSCRIPTION from ACC_NEWSLETTER 
				where DELIVERY_TYPE = 'Телефонный банкинг' 
				and ACC_CONTACT_INFOID = ACC_CONTACT_INFO.ACC_CONTACT_INFOID)
					as phoneBanking
			from ACC_CONTACT_INFO where ACCOUNTID = ${escape(key)}`,
			'$key, contactType, contactValue, isPrimary, marketing, newsletter, confidential, PFK, phoneBanking',
		);

		const contactList: Contact[] = await query;
		for (const object of contactList) {
			if (object.isPrimary == 'T') object.isPrimary = true;
			else object.isPrimary = false;
		}
		if (contactList && contactList.length > 0) {
			commit('LoadContacts', contactList);
		}

		query = api.select(
			`select ADDRESSID, DESCRIPTION, FULL_ADDRESS, ISPRIMARY, ISMAILING from ADDRESS where ENTITYID = ${escape(key)}`,
			'$key, description, fulladdress, primary, mailing',
		);

		const addressList: Address[] = await query;
		for (const object of addressList) {
			if (object.primary == 'T') object.primary = true;
			else object.primary = false;
			if (object.mailing == 'T') object.mailing = true;
			else object.mailing = false;
		}
		if (addressList && addressList.length > 0) {
			commit('LoadAddresses', addressList);
		}
		commit('FinalizeContactsTabLoading');
		return true;
	},
	async addHelper(
		{ commit },
		payload: { assistant: IACCOUNTASSISTANT; contacts: IAccContactInfo[]; addresses: IAddress[] },
	): Promise<boolean> {
		if (!payload) return false;

		let response = null;
		try {
			response = await api
				.create('accountAssistants', accountAssistantToServerView(payload.assistant))
				.then(x => x.json());
		} catch {
			return false;
		}
		const newHelper: IACCOUNTASSISTANT = accountAssistantToClientView(response, 'exist');

		if (payload.addresses.length > 0) {
			try {
				for (const address of payload.addresses) {
					address.EntityId = newHelper.$key;
					address.EntityType = 'AccountAssistant';
					response = await api.create('addresses', addressToServerView(address)).then(x => x.json());
					if (response.IsPrimary) {
						newHelper.$primaryAddress = response.FullAddress;
					}
				}
			} catch {
				return false;
			}
		}

		if (payload.contacts.length > 0) {
			try {
				for (const contact of payload.contacts) {
					contact.ACCOUNTASSISTANTID = newHelper.$key;
					response = await api.create('accContactInfoes', contactToServerView(contact)).then(x => x.json());
					if (contact.IsMain && contact.ContactType && contact.ContactType.includes('Тел')) {
						newHelper.$primaryPhone = contact;
					}
					if (
						contact.IsMain &&
						contact.ContactType &&
						(contact.ContactType.toLowerCase().includes('e-mail') ||
							contact.ContactType.toLowerCase().includes('email')) &&
						contact.ContactValue
					) {
						newHelper.$primaryEmail = contact;
					}

					if (contact.$communications && contact.$communications.length > 0) {
						for (const communication of contact.$communications) {
							communication.ACCCONTACTINFOID = response.$key;
							await api.create('accNewsletters', communicationToServerView(communication)).then(x => x.json());
						}
					}
				}
			} catch {
				return false;
			}
		}

		commit('AddHelper', newHelper);
		return true;
	},
	async updateHelper(
		{ commit },
		payload: { assistant: IACCOUNTASSISTANT; contacts: IAccContactInfo[]; addresses: IAddress[] },
	): Promise<boolean> {
		if (!payload || !payload.assistant || !payload.assistant.$key) return false;

		let response = null;
		try {
			response = await api
				.update('accountAssistants', payload.assistant.$key, accountAssistantToServerView(payload.assistant))
				.then(x => x.json());
		} catch {
			return false;
		}
		if (payload.assistant.$key) commit('RemoveHelper', payload.assistant.$key);

		if (payload.addresses && payload.addresses.length > 0) {
			try {
				for (const address of payload.addresses) {
					if (address.$state == 'updated') {
						if (address.$key)
							response = await api.update('addresses', address.$key, addressToServerView(address)).then(x => x.json());
					} else if (address.$state == 'new') {
						address.EntityId = payload.assistant.$key;
						address.EntityType = 'ACCOUNTASSISTANT';
						response = await api.create('addresses', addressToServerView(address)).then(x => x.json());
					}
					if (address.IsPrimary && address.FullAddress) {
						payload.assistant.$primaryAddress = address;
					}
				}
			} catch {
				return false;
			}
		}

		if (payload.contacts && payload.contacts.length > 0) {
			try {
				for (const contact of payload.contacts) {
					if (contact.$state == 'new') {
						contact.ACCOUNTASSISTANTID = payload.assistant.$key;
						response = await api.create('accContactInfoes', contactToServerView(contact)).then(x => x.json());
					} else if (contact.$state == 'updated' && contact.$key) {
						response = await api
							.update('accContactInfoes', contact.$key, contactToServerView(contact))
							.then(x => x.json());
					}
					if (contact.IsMain && contact.ContactType && contact.ContactType.includes('Тел') && contact.ContactValue) {
						payload.assistant.$primaryPhone = contact;
					}
					if (
						contact.IsMain &&
						contact.ContactType &&
						(contact.ContactType.toLowerCase().includes('e-mail') ||
							contact.ContactType.toLowerCase().includes('email')) &&
						contact.ContactValue
					) {
						payload.assistant.$primaryEmail = contact;
					}

					if (contact.$communications && contact.$communications.length > 0) {
						for (const communication of contact.$communications) {
							if (communication.$state == 'new') {
								communication.ACCCONTACTINFOID = response.$key;
								await api.create('accNewsletters', communicationToServerView(communication)).then(x => x.json());
							} else if (communication.$state == 'updated' && communication.$key) {
								await api
									.update('accNewsletters', communication.$key, communicationToServerView(communication))
									.then(x => x.json());
							}
						}
					}
				}
			} catch {
				return false;
			}
		}

		commit('AddHelper', payload.assistant);
		return true;
	},
	async saveContacts({ commit, state }): Promise<boolean> {
		if (!state._entity || !state._entity.accountExtension) return false;

		let response = null;
		try {
			if (state._entity.accountExtension.$key) {
				response = await api
					.update(
						'accountExts',
						state._entity.accountExtension.$key,
						accountExtToServerView(state._entity.accountExtension),
					)
					.then(x => x.json());
			}
		} catch {
			return false;
		}

		try {
			for (const helper of state._helpers) {
				if (helper.$state == 'updated' && helper.$key) {
					const serverHelper = accountAssistantToServerView(helper);
					response = await api.update('accountAssistants', helper.$key, serverHelper);
				}
			}
		} catch {
			return false;
		}

		try {
			for (const contact of state._contacts) {
				if (contact.$state == 'updated' && contact.$key) {
					const serverContact = contactToServerView({ IsMain: !!contact.isPrimary, $state: 'updated' });
					response = await api.update('accContactInfoes', contact.$key, serverContact);
				}
			}
		} catch {
			return false;
		}

		try {
			for (const address of state._addresses) {
				if (address.$state == 'updated' && address.$key) {
					const serverAddress = addressToServerView({
						IsPrimary: !!address.primary,
						IsMailing: !!address.mailing,
						$state: 'updated',
					});
					response = await api.update('addresses', address.$key, serverAddress);
				}
			}
		} catch {
			return false;
		}

		commit('SetAllHelpersExist');
		commit('SetAllContactsExist');
		commit('SetAllAddressesExist');

		if (!response || 'message' in response) return false;
		return true;
	},
	async deleteHelper({ commit }, payload: IACCOUNTASSISTANT): Promise<boolean> {
		if (!payload || !payload.$key) return false;

		let response = null;
		try {
			response = await api.delete('accountAssistants', payload.$key);
		} catch {
			return false;
		}
		if (!response || !response.ok) return false;
		commit('RemoveHelper', payload.$key);
		return true;
	},
	async addContact({ commit }, payload: IAccContactInfo): Promise<boolean> {
		if (!payload) return false;

		const contact: Contact = {};

		let response = null;
		try {
			console.log(payload);
			response = await api.create('accContactInfoes', contactToServerView(payload)).then(x => x.json());
			contact.$key = response.$key;
			if (payload.ContactType) contact.contactType = payload.ContactType;
			if (payload.ContactValue) contact.contactValue = payload.ContactValue;
			if (payload.IsMain) contact.isPrimary = payload.IsMain;

			if (payload.$communications && payload.$communications.length > 0) {
				for (const communication of payload.$communications) {
					communication.ACCCONTACTINFOID = response.$key;
					await api.create('accNewsletters', communicationToServerView(communication)).then(x => x.json());
					if (communication.DeliveryType == 'Маркетинг' && communication.Subscription)
						contact.marketing = communication.Subscription;
					if (communication.DeliveryType == 'Инф. рассылка' && communication.Subscription)
						contact.newsletter = communication.Subscription;
					if (communication.DeliveryType == 'Конфиденц. инф-ция' && communication.Subscription)
						contact.confidential = communication.Subscription;
					if (communication.DeliveryType == 'ПФК' && communication.Subscription)
						contact.PFK = communication.Subscription;
					if (communication.DeliveryType == 'Телефонный банкинг' && communication.Subscription)
						contact.phoneBanking = communication.Subscription;
				}
			}
		} catch {
			return false;
		}

		if (contact.isPrimary) {
			commit('EnsureContactPrimary', contact);
		}
		commit('AddContact', contact);
		return true;
	},
	async deleteContact({ commit }, payload: Contact): Promise<boolean> {
		if (!payload || !payload.$key) return false;

		let response = null;
		try {
			response = await api.delete('accContactInfoes', payload.$key);
		} catch {
			return false;
		}
		if (!response || !response.ok) return false;
		commit('RemoveContact', payload.$key);
		return true;
	},
	async updateContact({ state, commit, getters }, payload: IAccContactInfo): Promise<boolean> {
		if (!payload || !payload.$key) return false;
		let response = null;
		try {
			response = await api.update('accContactInfoes', payload.$key, contactToServerView(payload));
			if (payload.$communications && payload.$communications.length > 0) {
				for (const communication of payload.$communications) {
					if (communication.$state == 'new') {
						communication.ACCCONTACTINFOID = payload.$key;
						await api.create('accNewsletters', communicationToServerView(communication)).then(x => x.json());
					} else if (communication.$state == 'updated' && communication.$key) {
						await api
							.update('accNewsletters', communication.$key, communicationToServerView(communication))
							.then(x => x.json());
					}
				}
			}
		} catch {
			return false;
		}
		if (!response || !response.ok) return false;

		const tab: TabBase = getters.getTabByTabId('FbContacts');
		if (!tab || !tab.isActive) return true;

		commit('RemoveContact', payload.$key);
		const newContact: Contact = {};
		newContact.$key = payload.$key;
		if (payload.ContactType) newContact.contactType = payload.ContactType;
		if (payload.ContactValue) newContact.contactValue = payload.ContactValue;
		if (payload.IsMain) newContact.isPrimary = payload.IsMain;
		if (payload.$communications && payload.$communications.length > 0) {
			for (const communication of payload.$communications) {
				if (communication.DeliveryType == 'Маркетинг' && communication.Subscription)
					newContact.marketing = communication.Subscription;
				if (communication.DeliveryType == 'Инф. рассылка' && communication.Subscription)
					newContact.newsletter = communication.Subscription;
				if (communication.DeliveryType == 'Конфиденц. инф-ция' && communication.Subscription)
					newContact.confidential = communication.Subscription;
				if (communication.DeliveryType == 'ПФК' && communication.Subscription)
					newContact.PFK = communication.Subscription;
				if (communication.DeliveryType == 'Телефонный банкинг' && communication.Subscription)
					newContact.phoneBanking = communication.Subscription;
			}
		}
		if (newContact.isPrimary) {
			commit('EnsureContactPrimary', newContact);
		}
		commit('AddContact', newContact);
		if (payload.ContactType && payload.ContactType.includes('Тел')) {
			if (state._entity && state._entity.officialPhone && state._entity.officialPhone.$key == payload.$key) {
				commit('UpdateOfficialPhone', payload);
			}
			if (state._entity && state._entity.privatePhone && state._entity.privatePhone.$key == payload.$key) {
				commit('UpdatePrivatePhone', payload);
			}
		}
		if (
			payload.ContactType &&
			(payload.ContactType.toLowerCase().includes('e-mail') || payload.ContactType.toLowerCase().includes('email'))
		) {
			if (state._entity && state._entity.email && state._entity.email.$key == payload.$key) {
				commit('UpdateEmail', payload);
			}
		}
		return true;
	},
	async addAddress({ commit }, payload: IAddress): Promise<boolean> {
		if (!payload) return false;

		const address: Address = {};

		let response = null;
		try {
			response = await api.create('addresses', addressToServerView(payload)).then(x => x.json());
			address.$key = response.$key;
			if (payload.Description) address.description = payload.Description;
			if (payload.FullAddress) address.fulladdress = payload.FullAddress;
			if (payload.IsPrimary) address.primary = payload.IsPrimary;
			if (payload.IsMailing) address.mailing = payload.IsMailing;
		} catch {
			return false;
		}

		commit('AddAddress', address);
		return true;
	},
	async updateAddress({ commit }, payload: IAddress): Promise<boolean> {
		if (!payload || !payload.$key) return false;

		let response = null;
		try {
			response = await api.update('addresses', payload.$key, addressToServerView(payload));
			console.log(response, addressToServerView(payload));
		} catch {
			return false;
		}
		if (!response || !response.ok) return false;
		commit('RemoveAddress', payload.$key);
		const newAddress: Address = {};
		newAddress.$key = payload.$key;
		if (payload.Description) newAddress.description = payload.Description;
		if (payload.FullAddress) newAddress.fulladdress = payload.FullAddress;
		if (payload.IsPrimary) newAddress.primary = payload.IsPrimary;
		if (payload.IsMailing) newAddress.mailing = payload.IsMailing;
		commit('AddAddress', newAddress);
		return true;
	},
	async deleteAddress({ commit }, payload: IAddress): Promise<boolean> {
		if (!payload || !payload.$key) return false;

		let response = null;
		try {
			response = await api.delete('addresses', payload.$key);
		} catch {
			return false;
		}
		if (!response || !response.ok) return false;
		commit('RemoveAddress', payload.$key);
		return true;
	},

	async loadFilials({ commit }, key: EntityId): Promise<boolean> {
		if (!key) return false;

		const query = api.select(
			queryFilials() + `WHERE A1.ACCOUNTID = ${escape(key)};`,
			'$key, type, managerFilialId, managerFilialName, managerCity, managerPlatform, code, isMainCity, serviceStartDate, managerId, managerName, dateOfAssignment',
		);

		let filialList: Filial[] = await query;
		filialList = filialList.filter(x => x.serviceStartDate && !x.serviceEndDate);
		filialList.forEach(x => (x = filialToClientView(x)));
		if (filialList && filialList.length > 0) {
			commit('LoadFilials', filialList);
		}

		commit('FinalizeFilialsTabLoading');
		return true;
	},
	async addFilial({ commit }, payload: { newFilial: Filial; accountId: string }): Promise<boolean> {
		if (!payload) return false;
		const accFilial: IAccFilial = {
			$state: 'new',
			AccountId: payload.accountId,
			UserId: payload.newFilial.managerId,
			OrgUnitId: payload.newFilial.managerFilialId,
			IsMain: payload.newFilial.isMainCity as boolean,
			ServiceStartDate: DateTime.utc(),
			AssignmentDate: DateTime.utc(),
		};

		let response = null;
		try {
			response = await api.create('accFilials', accFilialtoServerView(accFilial)).then(x => x.json());
		} catch {
			return false;
		}

		const newFilial = accFilialToClientView(response, 'exist');

		const query = api.select(
			queryFilials() + `WHERE A1.ACCOUNTID = ${escape(payload.accountId)};`,
			'$key, type, managerFilialId, managerFilialName, managerCity, managerPlatform, code, isMainCity, serviceStartDate, managerId, managerName, dateOfAssignment',
		);
		let filialList: Filial[] = await query;
		filialList = filialList.filter(x => x.serviceStartDate && !x.serviceEndDate);
		filialList.forEach(x => (x = filialToClientView(x)));
		commit(
			'AddFilial',
			filialList.find(x => x.$key === newFilial.$key),
		);
		filialList.filter(x => x.$key !== newFilial.$key).forEach(x => commit('UpdateFilial', x));
		return true;
	},
	async updateFilial({ commit }, payload: { filialToUpdate: Filial; accountId: string }): Promise<boolean> {
		if (!payload) return false;
		let accFilial: IAccFilial = await api.read('accFilials', payload.filialToUpdate.$key as string).then(x => x.json());
		accFilial = accFilialToClientView(accFilial, 'exist');

		if (accFilial.UserId !== payload.filialToUpdate.managerId) {
			accFilial.AssignmentDate = payload.filialToUpdate.dateOfAssignment as DateTime;
		}

		accFilial.IsMain = payload.filialToUpdate.isMainCity as boolean;
		accFilial.UserId = payload.filialToUpdate.managerId;
		accFilial.OrgUnitId = payload.filialToUpdate.managerFilialId;

		accFilial.$state = 'updated';
		let response = null;
		try {
			response = await api.update('accFilials', accFilial.$key as string, accFilialtoServerView(accFilial));
		} catch {
			return false;
		}
		if (!response || !response.ok) return false;

		const query = api.select(
			queryFilials() + `WHERE A1.ACCOUNTID = ${escape(payload.accountId)};`,
			'$key, type, managerFilialId, managerFilialName, managerCity, managerPlatform, code, isMainCity, serviceStartDate, managerId, managerName, dateOfAssignment',
		);
		let filialList: Filial[] = await query;
		filialList = filialList.filter(x => x.serviceStartDate && !x.serviceEndDate);
		filialList.forEach(x => (x = filialToClientView(x)));
		filialList.forEach(x => commit('UpdateFilial', x));

		return true;
	},

	async deleteFilial({ commit }, payload: Filial): Promise<boolean> {
		if (!payload || !payload.$key) return false;

		let response = null;
		try {
			response = await api.delete('accFilials', payload.$key);
		} catch {
			return false;
		}
		if (!response || !response.ok) return false;
		commit('RemoveFilial', payload.$key);
		return true;
	},

};

const AccountModule: Module<typeof state, RootState> = {
	namespaced,
	state,
	getters,
	actions,
	mutations,
};

export default AccountModule;
