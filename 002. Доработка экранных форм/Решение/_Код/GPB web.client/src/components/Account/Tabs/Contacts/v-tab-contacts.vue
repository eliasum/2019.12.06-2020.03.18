// !THOU SHALL SUFFER WHILE READING THIS
// !НЕ ПОВТОРЯТЬ
<template>
	<div>
		<div class="v-tab-contacts" v-if="page.account">
			<div class="v-tab-contacts-mainarea">
				<v-label text="Основное общение через помощника:" element="chk_isHelperMain" />
				<v-checkbox id="chk_isHelperMain" :value="helperMain" @change="helperMainChange()" noLabel size="small" />
			</div>
			<v-accordion caption="Помощники и их контакты" :expanded="helperMain" v-if="contactsTabLoaded">
				<template v-slot:buttons>
					<v-tooltip direction="top">
						<v-button circle flat class="v-accordion-buttonContainer-button" @click="addHelper($event)">{{'&#10010;'}}</v-button>
						<template #tooltip>{{'Добавить'}}</template>
					</v-tooltip>
					<v-tooltip direction="top">
						<v-button circle flat class="v-accordion-buttonContainer-button" @click.stop="editHelper()">{{'&#9998;'}}</v-button>
						<template #tooltip>{{'Редактировать'}}</template>
					</v-tooltip>
					<v-tooltip direction="top">
						<v-button circle flat class="v-accordion-buttonContainer-button" @click="deleteHelper($event)">{{'&#8722;'}}</v-button>
						<template #tooltip>{{'Удалить'}}</template>
					</v-tooltip>
				</template>
				<v-data-table
					ref="helpersGrid"
					class="contactsGrid"
					:data="helpers"
					idField="$key"
					:columns="helperColumns"
					renderMode="all"
					narrow
					v-model="selectedHelper"
					selectionMode="single"
					:height="125"
				>
					<template #default="{entity}">
						<td class="v-data-table-td--0">
							<v-link :text="entity.FULLNAME" @click="editHelper(entity)" />
						</td>
						<td class="v-data-table-td--1">
							<v-checkbox noLabel size="small" :value="entity.ISMAIN" @change="primaryHelperChanged(entity, $event)" />
						</td>
						<td class="v-data-table-td--2">
							<v-link :text="getHelperPrimaryPhoneText(entity)" @click="helperPrimaryPhoneLnkClick(entity)" />
						</td>
						<td class="v-data-table-td--3">
							<v-link :text="getHelperPrimaryEmailText(entity)" @click="helperPrimaryEmailLnkClick(entity)" />
						</td>
						<td class="v-data-table-td--4">{{getHelperPrimaryAddressText(entity)}}</td>
					</template>
				</v-data-table>
			</v-accordion>
			<v-loader v-else />
			<v-accordion caption="Контакты клиента (телефон, e-mail)" :expanded="true" v-if="contactsTabLoaded" :active="false">
				<template v-slot:buttons>
					<v-tooltip direction="top">
						<v-button circle flat class="v-accordion-buttonContainer-button" @click="addContact($event)">{{'&#10010;'}}</v-button>
						<template #tooltip>{{'Добавить'}}</template>
					</v-tooltip>
					<v-tooltip direction="top">
						<v-button circle flat class="v-accordion-buttonContainer-button" @click="editContact($event)">{{'&#9998;'}}</v-button>
						<template #tooltip>{{'Редактировать'}}</template>
					</v-tooltip>
					<v-tooltip direction="top">
						<v-button circle flat class="v-accordion-buttonContainer-button" @click="deleteContact($event)">{{'&#8722;'}}</v-button>
						<template #tooltip>{{'Удалить'}}</template>
					</v-tooltip>
				</template>
				<v-data-table
					ref="contactsGrid"
					class="contactsGrid"
					:data="contacts"
					idField="$key"
					:columns="contactColumns"
					renderMode="all"
					narrow
					:height="125"
					v-model="selectedContact"
					selectionMode="single"
				>
					<template #default="{entity}">
						<td class="v-data-table-td--0">{{entity.contactType}}</td>
						<td class="v-data-table-td--1">{{entity.contactValue}}</td>
						<td class="v-data-table-td--2">
							<v-checkbox noLabel size="small" :value="entity.isPrimary" @change="primaryContactChanged(entity, $event)" />
						</td>
						<td class="v-data-table-td--3">
							<img src="images\FbConsult\yes.png" v-if="entity.marketing == 'Согласие'" />
							<img src="images\FbConsult\no.png" v-if="entity.marketing == 'Отказ'" />
						</td>
						<td class="v-data-table-td--4">
							<img src="images\FbConsult\yes.png" v-if="entity.newsletter == 'Согласие'" />
							<img src="images\FbConsult\no.png" v-if="entity.newsletter == 'Отказ'" />
						</td>
						<td class="v-data-table-td--5">
							<img src="images\FbConsult\yes.png" v-if="entity.confidential == 'Согласие'" />
							<img src="images\FbConsult\no.png" v-if="entity.confidential == 'Отказ'" />
						</td>
						<td class="v-data-table-td--6">
							<img src="images\FbConsult\yes.png" v-if="entity.PFK == 'Согласие'" />
							<img src="images\FbConsult\no.png" v-if="entity.PFK == 'Отказ'" />
						</td>
						<td class="v-data-table-td--7">
							<img src="images\FbConsult\yes.png" v-if="entity.phoneBanking == 'Согласие'" />
							<img src="images\FbConsult\no.png" v-if="entity.phoneBanking == 'Отказ'" />
						</td>
					</template>
				</v-data-table>
			</v-accordion>
			<v-loader v-else />
			<v-accordion caption="Контакты клиента (адреса)" :expanded="true" v-if="contactsTabLoaded" :active="false">
				<template v-slot:buttons>
					<v-tooltip direction="top">
						<v-button circle flat class="v-accordion-buttonContainer-button" @click="addAddress($event)">{{'&#10010;'}}</v-button>
						<template #tooltip>{{'Добавить'}}</template>
					</v-tooltip>
					<v-tooltip direction="top">
						<v-button circle flat class="v-accordion-buttonContainer-button" @click="editAddress($event)">{{'&#9998;'}}</v-button>
						<template #tooltip>{{'Редактировать'}}</template>
					</v-tooltip>
					<v-tooltip direction="top">
						<v-button circle flat class="v-accordion-buttonContainer-button" @click="deleteAddress($event)">{{'&#8722;'}}</v-button>
						<template #tooltip>{{'Удалить'}}</template>
					</v-tooltip>
				</template>
				<v-data-table
					ref="addressesGrid"
					class="addressesGrid"
					:data="addresses"
					idField="$key"
					:columns="addressColumns"
					renderMode="all"
					narrow
					:height="125"
					v-model="selectedAddress"
					selectionMode="single"
				>
					<template #default="{entity}">
						<td class="v-data-table-td--0">{{entity.description}}</td>
						<td class="v-data-table-td--1">{{entity.fulladdress}}</td>
						<td class="v-data-table-td--2">
							<v-checkbox noLabel size="small" :value="entity.primary" @change="primaryAddressChanged(entity, $event)" />
						</td>
						<td class="v-data-table-td--3">
							<v-checkbox noLabel size="small" :value="entity.mailing" @change="mailingAddressChanged(entity, $event)" />
						</td>
					</template>
				</v-data-table>
			</v-accordion>
			<v-loader v-else />
		</div>
		<v-loader v-else />
		<v-tab-content-toolbar>
			<v-tooltip direction="top">
				<v-button circle flat @click="save">&#128190;</v-button>
				<template #tooltip>{{'Сохранить'}}</template>
			</v-tooltip>
		</v-tab-content-toolbar>

		<v-modal caption="Добавить/Изменить помощника" showOk :visible="helpersDialogVisible" :width="1000" @close="helpersDialogClosed($event)">
			<div class="dialog-helpers-main">
				<div class="dialog-helpers-main-left">
					<div class="dialog-helpers-main-left-fullname">
						<div class="dialog-helpers-main-left-fullname-label">
							<v-label :text="'ФИО:'" element="dlgHelpers-txt_Fullname" />
						</div>
						<div class="dialog-helpers-main-left-fullname-input">
							<v-text-input
								id="dlgHelpers-txt_Fullname"
								bordered
								narrow
								noLabel
								:value="helperFocused && helperFocused.FULLNAME"
								@change="helperFocused.FULLNAME = $event"
							/>
						</div>
					</div>
					<div class="dialog-helpers-main-left-organization">
						<div class="dialog-helpers-main-left-organization-label">
							<v-label :text="'Компания:'" element="dlgHelpers-pkl_Organization" />
						</div>
						<div class="dialog-helpers-main-left-organization-input">
							<v-picklist
								id="dlgHelpers-pkl_Organization"
								bordered
								narrow
								noLabel
								picklistName="Юридические лица"
								:value="helperFocused && helperFocused.COMPANYNAME"
								@change="helperFocused.COMPANYNAME = $event"
								notShowOnInputClick
								searchable
								inputable
								emitsIds
								@changeId="dlgHelpers_itemId = $event"
							/>
						</div>
					</div>
					<div class="dialog-helpers-main-left-department">
						<div class="dialog-helpers-main-left-department-label">
							<v-label :text="'Отдел:'" element="dlgHelpers-txt_Department" />
						</div>
						<div class="dialog-helpers-main-left-department-input">
							<v-text-input
								id="dlgHelpers-txt_Department"
								bordered
								narrow
								noLabel
								:value="helperFocused && helperFocused.DIVISION"
								@change="helperFocused.DIVISION = $event"
							/>
						</div>
					</div>
					<div class="dialog-helpers-main-left-title">
						<div class="dialog-helpers-main-left-title-label">
							<v-label :text="'Должность:'" element="dlgHelpers-txt_Title" />
						</div>
						<div class="dialog-helpers-main-left-title-input">
							<v-text-input
								id="dlgHelpers-txt_Title"
								bordered
								narrow
								noLabel
								:value="helperFocused && helperFocused.TITLE"
								@change="helperFocused.TITLE = $event"
							/>
						</div>
					</div>
				</div>
				<div class="dialog-helpers-main-right">
					<div class="dialog-helpers-main-right-isPrimary">
						<div class="dialog-helpers-main-right-isPrimary-label">
							<v-label :text="'Основной помощник:'" element="dlgHelpers-chk_IsPrimary" />
						</div>
						<div class="dialog-helpers-main-right-isPrimary-chk">
							<v-checkbox id="dlgHelpers-chk_IsPrimary" noLabel size="small" v-model="helperFocused.ISMAIN" v-if="helperFocused" />
						</div>
					</div>
					<div class="dialog-helpers-main-right-comment">
						<div class="dialog-helpers-main-right-comment-label">
							<v-label :text="'Комментарии к помощнику'" element="dlgHelpers-txt_Comments" />
						</div>
						<div class="dialog-helpers-main-right-comment-text">
							<v-textarea
								id="dlgHelpers-txt_Comments"
								noLabel
								:placeholder="'Произвольный текст'"
								:rows="3"
								:value="helperFocused && helperFocused.Notes"
								@change="helperFocused.Notes = $event"
							/>
						</div>
					</div>
				</div>
			</div>
			<div class="dialog-helpers-contacts1">
				<v-accordion caption="Контакты помощника (телефон, e-mail)" :active="false">
					<template v-slot:buttons>
						<v-tooltip direction="top">
							<v-button circle flat class="v-accordion-buttonContainer-button" @click="dlgHelpersAddContact($event)">{{'&#10010;'}}</v-button>
							<template #tooltip>{{'Добавить'}}</template>
						</v-tooltip>
						<v-tooltip direction="top">
							<v-button circle flat class="v-accordion-buttonContainer-button" @click="dlgHelpersEditContact($event)">{{'&#9998;'}}</v-button>
							<template #tooltip>{{'Редактировать'}}</template>
						</v-tooltip>
						<v-tooltip direction="top">
							<v-button circle flat class="v-accordion-buttonContainer-button" @click="dlgHelpersDeleteContact($event)">{{'&#8722;'}}</v-button>
							<template #tooltip>{{'Удалить'}}</template>
						</v-tooltip>
					</template>
					<v-data-table
						:data="dlgHelpersContacts"
						idField="ContactValue"
						:columns="dlgHelpersContactsColumns"
						renderMode="pagination"
						narrow
						:height="200"
						v-model="dlgHelpersSelectedContact"
						selectionMode="single"
					>
						<template #default="{entity}">
							<td class="v-data-table-td--0">{{entity.ContactType}}</td>
							<td class="v-data-table-td--1">{{entity.ContactValue}}</td>
							<td class="v-data-table-td--2">
								<v-checkbox noLabel size="small" :value="entity.IsMain" @change="dlgHelpersMainContactChanged(entity, $event)" @click.stop />
							</td>
							<td class="v-data-table-td--3">{{entity.Comments}}</td>
						</template>
					</v-data-table>
				</v-accordion>
			</div>
			<div class="dialog-helpers-contacts2">
				<v-accordion caption="Контакты помощника (адреса)" :active="false">
					<template v-slot:buttons>
						<v-tooltip direction="top">
							<v-button circle flat class="v-accordion-buttonContainer-button" @click="dlgHelpersAddAddress($event)">{{'&#10010;'}}</v-button>
							<template #tooltip>{{'Добавить'}}</template>
						</v-tooltip>
						<v-tooltip direction="top">
							<v-button circle flat class="v-accordion-buttonContainer-button" @click="dlgHelpersEditAddress($event)">{{'&#9998;'}}</v-button>
							<template #tooltip>{{'Редактировать'}}</template>
						</v-tooltip>
						<v-tooltip direction="top">
							<v-button circle flat class="v-accordion-buttonContainer-button" @click="dlgHelpersDeleteAddress($event)">{{'&#8722;'}}</v-button>
							<template #tooltip>{{'Удалить'}}</template>
						</v-tooltip>
					</template>
					<v-data-table
						:data="dlgHelpersAddresses"
						idField="FullAddress"
						:columns="dlgHelpersAddressColumns"
						renderMode="pagination"
						narrow
						:height="200"
						v-model="dlgHelpersSelectedAddress"
						selectionMode="single"
					>
						<template #default="{entity}">
							<td class="v-data-table-td--0">{{entity.Description}}</td>
							<td class="v-data-table-td--1">{{entity.FullAddress}}</td>
							<td class="v-data-table-td--2">
								<v-checkbox noLabel size="small" :value="entity.IsPrimary" @change="dlgHelpersPrimaryAddressChanged(entity, $event)" />
							</td>
							<td class="v-data-table-td--3">
								<v-checkbox noLabel size="small" :value="entity.IsMailing" @change="dlgHelpersMailingAddressChanged(entity, $event)" />
							</td>
						</template>
					</v-data-table>
				</v-accordion>
			</div>
		</v-modal>

		<v-dialog-contact-addedit
			v-if="contactFocused"
			:visible="contactDialogVisible"
			:contact="contactFocused"
			@change="contactDialogChange($event)"
			@close="contactDialogClosed()"
		/>

		<v-modal
			caption="Добавить/Изменить контактный адрес"
			showOk
			:visible="addressDialogVisible"
			@close="addressDialogClosed($event)"
			:width="500"
		>
			<div class="dialog-address" v-if="addressFocused">
				<div class="dialog-address-type">
					<div class="dialog-address-type-lbl">
						<v-label :text="'Тип адреса:'" element="dlgAddress-pkl_Type" input />
					</div>
					<div class="dialog-address-type-pkl">
						<v-picklist id="dlgAddress-pkl_Type" bordered narrow noLabel picklistName="Тип адреса" v-model="addressFocused.Description" />
					</div>
				</div>
				<div class="dialog-address-primary">
					<div class="dialog-address-primary-lbl">
						<v-label :text="'Основной:'" element="dlgAddress-chk_Primary" />
					</div>
					<div class="dialog-address-primary-chk">
						<v-checkbox id="dlgAddress-chk_Primary" noLabel size="small" v-model="addressFocused.IsPrimary" />
					</div>
				</div>
				<div class="dialog-address-mailing">
					<div class="dialog-address-mailing-lbl">
						<v-label :text="'Почтовый:'" element="dlgAddress-chk_Mailing" />
					</div>
					<div class="dialog-address-mailing-check">
						<v-checkbox id="dlgAddress-chk_Mailing" noLabel size="small" v-model="addressFocused.IsMailing" />
					</div>
				</div>
				<div class="dialog-address-index">
					<div class="dialog-address-index-lbl">
						<v-label :text="'Индекс:'" element="dlgAddress-txt_Index" input />
					</div>
					<div class="dialog-address-index-txt">
						<v-text-input id="dlgAddress-txt_Index" bordered narrow noLabel v-model="addressFocused.PostalCode" />
					</div>
				</div>
				<div class="dialog-address-country">
					<div class="dialog-address-country-lbl">
						<v-label :text="'Страна:'" element="dlgAddress-txt_Country" input />
					</div>
					<div class="dialog-address-country-txt">
						<v-text-input id="dlgAddress-txt_Country" bordered narrow noLabel v-model="addressFocused.Country" />
					</div>
				</div>
				<div class="dialog-address-subject">
					<div class="dialog-address-subject-lbl">
						<v-label :text="'Субъект РФ:'" element="dlgAddress-txt_Subject" input />
					</div>
					<div class="dialog-address-subject-txt">
						<v-text-input id="dlgAddress-txt_Subject" bordered narrow noLabel v-model="addressFocused.State" />
					</div>
				</div>
				<div class="dialog-address-region">
					<div class="dialog-address-region-lbl">
						<v-label :text="'Регион:'" element="dlgAddress-txt_Region" input />
					</div>
					<div class="dialog-address-region-txt">
						<v-text-input id="dlgAddress-txt_Region" bordered narrow noLabel v-model="addressFocused.County" />
					</div>
				</div>
				<div class="dialog-address-city">
					<div class="dialog-address-city-lbl">
						<v-label :text="'Город:'" element="dlgAddress-txt_City" input />
					</div>
					<div class="dialog-address-city-txt">
						<v-text-input id="dlgAddress-txt_Region" bordered narrow noLabel v-model="addressFocused.City" />
					</div>
				</div>
				<div class="dialog-address-street">
					<div class="dialog-address-street-lbl">
						<v-label :text="'Улица/Квартал:'" element="dlgAddress-txt_Street" input />
					</div>
					<div class="dialog-address-street-txt">
						<v-text-input id="dlgAddress-txt_Street" bordered narrow noLabel v-model="addressFocused.Street" />
					</div>
				</div>
				<div class="dialog-address-house">
					<div class="dialog-address-house-lbl">
						<v-label :text="'Дом/Владение:'" element="dlgAddress-txt_House" input />
					</div>
					<div class="dialog-address-house-txt">
						<v-text-input id="dlgAddress-txt_House" bordered narrow noLabel v-model="addressFocused.House" />
					</div>
				</div>
				<div class="dialog-address-building">
					<div class="dialog-address-building-lbl">
						<v-label :text="'Корпус/Строение:'" element="dlgAddress-txt_Building" input />
					</div>
					<div class="dialog-address-building-txt">
						<v-text-input id="dlgAddress-txt_Building" bordered narrow noLabel v-model="addressFocused.Building" />
					</div>
				</div>
				<div class="dialog-address-apartment">
					<div class="dialog-address-apartment-lbl">
						<v-label :text="'Офис/Квартира:'" element="dlgAddress-txt_Apartment" input />
					</div>
					<div class="dialog-address-apartment-txt">
						<v-text-input id="dlgAddress-txt_Apartment" bordered narrow noLabel v-model="addressFocused.Office" />
					</div>
				</div>
				<div class="dialog-address-fulladdress">
					<div class="dialog-address-fulladdress-lbl">
						<v-label :text="'Адрес одной строкой:'" element="dlgAddress-txt_Fulladdress" />
					</div>
					<div class="dialog-address-fulladdress-txt">
						<v-textarea id="dlgAddress-txt_Fulladdress" noLabel :rows="3" v-model="addressFocused.FullAddress" />
					</div>
				</div>
				<div class="dialog-address-source">
					<div class="dialog-address-source-lbl">
						<v-label :text="'Источник:'" element="dlgAddress-txt_Source" input />
					</div>
					<div class="dialog-address-source-txt">
						<v-text-input id="dlgAddress-txt_Source" bordered narrow noLabel v-model="addressFocused.SourceName" readonly />
					</div>
				</div>
			</div>
		</v-modal>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import TabBase from '@/base-components/abstract/v-tab-base';
import { SubtabComponent } from '@/base-components/v-subtab';
import store from '@/store';
import { AccountExtended } from '@/store/Account.store';
import ButtonTemplate from '@/model/client/ButtonTemplate';
import { IACCOUNTASSISTANT, toClientView as accountAssistantToClientView } from '@/model/interfaces/IAccountAssistant';
import { IAccContactInfo, toClientViewFromSelect as contactToClientView } from '@/model/interfaces/IAccContactInfo';
import { IAddress, toClientViewFromSelect as addressToClientView } from '@/model/interfaces/IAddress';
import { IAccNewsletter, toClientViewFromSelect as communicationToClientView } from '@/model/interfaces/IAccNewsletter';
import { EntityId } from '@/model/interfaces/EntityBase';
import DataTableComponent from '@/base-components/v-data-table.vue';
import Helper from '@/model/client/Helpers';
import Contact from '@/model/client/Contact';
import { IPickList } from '@/model/interfaces/IPicklist';
import PicklistComponent from '@/base-components/v-picklist.vue';
import Address from '@/model/client/Address';
import LockQueue from '../../../../decorators/LockQueue';
import Lock from '../../../../decorators/Lock';

@Component
export default class TabContacts extends TabBase {
	private helpersDialogVisible = false;
	private contactDialogVisible = false;
	private addressDialogVisible = false;

	private helperFocused: IACCOUNTASSISTANT | null = null;
	private contactFocused: IAccContactInfo | null = null;
	private addressFocused: IAddress | null = null;

	private dlgHelpersContacts: IAccContactInfo[] = [];
	private dlgHelpersAddresses: IAddress[] = [];
	private dlgHelpers_itemId = '';

	private dlgHelpersSelectedContact: IAccContactInfo[] | null = null;
	private dlgHelpersSelectedAddress: IAddress[] | null = null;
	private selectedHelper: IACCOUNTASSISTANT[] | null = null;
	private selectedContact: Contact[] | null = null;
	private selectedAddress: Address[] | null = null;

	private dlgContactEditMode = false;
	private dlgAddressEditMode = false;

	$refs!: {
		helpersGrid: DataTableComponent;
		contactsGrid: DataTableComponent;
		addressesGrid: DataTableComponent;
	};

	private get helpers(): IACCOUNTASSISTANT[] {
		return (this.$store as typeof store).getters[this.page.storeName + '/helpers'];
	}

	private get contacts(): Contact[] {
		return (this.$store as typeof store).getters[this.page.storeName + '/contacts'];
	}

	private get addresses(): Address[] {
		return (this.$store as typeof store).getters[this.page.storeName + '/addresses'];
	}

	private get account(): AccountExtended {
		return (this.$store as typeof store).getters[this.page.storeName + '/entity'];
	}

	private get helperMain() {
		return this.account.accountExtension.IsThroughAssistantComm === 'T';
	}

	private get contactsTabLoaded() {
		return (this.$store as typeof store).getters[this.page.storeName + '/contactsTabLoaded'];
	}

	private helperMainChange() {
		this.$store.dispatch(this.page.storeName + '/helperMainUpdate', !this.helperMain);
	}

	@LockQueue(true)
	protected async created() {
		await super.created();
		await this.$store.dispatch(this.page.storeName + '/loadHelpersContacts', this.page.entityId);
	}

	@Lock
	private async save() {
		if (!(await this.$store.dispatch(this.page.storeName + '/saveContacts', this.page.entityId)))
			this.$alert('Ошибка при обновлении сущности');
		else {
			this.$refs.helpersGrid.$forceUpdate();
			this.$refs.contactsGrid.$forceUpdate();
			this.$refs.addressesGrid.$forceUpdate();
		}
	}

	private get helperColumns() {
		return [
			{ fieldName: 'fullname', caption: 'ФИО помощника', isDisplayName: true },
			{ fieldName: 'isPrimary', caption: 'Основной' },
			{ fieldName: 'phone', caption: 'Основной телефон' },
			{ fieldName: 'email', caption: 'Основной e-mail' },
			{ fieldName: 'address', caption: 'Основной адрес' },
		];
	}

	private get contactColumns() {
		return [
			{ fieldName: 'contactType', caption: 'Тип контакта' },
			{ fieldName: 'contactValue', caption: 'Контакт' },
			{ fieldName: 'isPrimary', caption: 'Основной', width: 115 },
			{
				fieldName: 'marketing',
				caption: 'Маркетинг',
				width: 115,
				tooltip: 'Маркетинговые материалы о продуктах, услугах и специальных акциях Банка/Партнёров Банка',
			},
			{
				fieldName: 'newsletter',
				caption: 'Инф. рассылка',
				width: 150,
				tooltip:
					'Сведения о продуктах и услугах Банка (включая, но не ограничиваясь, информацией об изменении тарифов, датах и суммах платежей по кредитам, возникновении просроченной задолженности в рамках заключенных договоров)',
			},
			{
				fieldName: 'confidential',
				caption: 'Конфиденц. информация',
				width: 220,
				tooltip:
					'Сведения о денежных средствах на счетах, проведенных операциях и других данных в рамках заключенных договоров расчетно-кассового и брокерского обслуживания',
			},
			{
				fieldName: 'PFK',
				caption: 'ПФК',
				width: 80,
				tooltip:
					'Индивидуальные инвестиционные рекомендации (включая Анкету и результирующую часть, с присвоенным инвестиционным профилем), а также любые иные документы в рамках Договора о ПФК',
			},
			{
				fieldName: 'phoneBanking',
				caption: 'Телефонный банкинг',
				width: 190,
				tooltip: 'Предоставление услуг Телефонного банкинга',
			},
		];
	}

	private get addressColumns() {
		return [
			{ fieldName: 'description', caption: 'Тип адреса' },
			{ fieldName: 'fulladdress', caption: 'Адрес' },
			{ fieldName: 'primary', caption: 'Основной' },
			{ fieldName: 'mailing', caption: 'Почтовый' },
		];
	}

	private get dlgHelpersContactsColumns() {
		return [
			{ fieldName: 'ContactType', caption: 'Тип контакта' },
			{ fieldName: 'ContactValue', caption: 'Контакт' },
			{ fieldName: 'IsMain', caption: 'Основной' },
			{ fieldName: 'Comments', caption: 'Комментарии к коммуникации', width: 282 },
		];
	}

	private get dlgHelpersAddressColumns() {
		return [
			{ fieldName: 'Description', caption: 'Тип адреса' },
			{ fieldName: 'FullAddress', caption: 'Адрес' },
			{ fieldName: 'IsPrimary', caption: 'Основной' },
			{ fieldName: 'IsMailing', caption: 'Курьер' },
		];
	}

	@Lock
	private addHelper(event: MouseEvent) {
		this.helperFocused = {
			$state: 'new',
			ACCOUNTID: this.page.entityId,
			ISMAIN: false,
			FULLNAME: '',
			COMPANYNAME: '',
			DIVISION: '',
			TITLE: '',
		};
		this.helpersDialogVisible = true;
		event.stopPropagation();
	}

	@Lock
	private async editHelper(helper: IACCOUNTASSISTANT | null = null) {
		const key: EntityId = '';
		if (helper && helper.$key) this.helperFocused = helper;
		else if (this.selectedHelper && this.selectedHelper[0] && this.selectedHelper[0].$key)
			this.helperFocused = this.selectedHelper[0];
		else return;
		this.helperFocused.$OLDCOMPANYNAME = this.helperFocused.COMPANYNAME;
		//Адреса
		let queryList = await this.$api.select(
			`select ADDRESSID, ENTITYTYPE, DESCRIPTION, FULL_ADDRESS, ISPRIMARY, ISMAILING, POSTALCODE, COUNTRY, STATE, COUNTY, CITY, STREET, HOUSE, BUILDING, OFFICE, SOURCE_NAME from ADDRESS where ENTITYID = '${this.helperFocused.$key}'`,
			'$key, EntityType, Description, FullAddress, IsPrimary, IsMailing, PostalCode, Country, State, County, City, Street, House, Building, Office, SourceName',
		);
		for (const queryItem of queryList) {
			this.dlgHelpersAddresses.push(addressToClientView(queryItem, 'exist'));
		}
		//Контакты
		queryList = await this.$api.select(
			`select ACC_CONTACT_INFOID, CONTACT_TYPE, CONTACT_VALUE, IS_MAIN, COMMENTS, SOURCE_NAME from ACC_CONTACT_INFO where ACCOUNT_ASSISTANTID = '${this.helperFocused.$key}'`,
			'$key, ContactType, ContactValue, IsMain, Comments, SourceName',
		);
		for (const queryItem of queryList) {
			const subqueryList = await this.$api.select(
				`select ACC_NEWSLETTERID, DELIVERY_TYPE, SUBSCRIPTION from ACC_NEWSLETTER where ACC_CONTACT_INFOID = '${queryItem.$key}'`,
				'$key, DeliveryType, Subscription',
			);
			const communications: IAccNewsletter[] = [];
			if (subqueryList.length > 0) {
				for (const subqueryItem of subqueryList) communications.push(communicationToClientView(subqueryItem, 'exist'));
			}
			this.dlgHelpersContacts.push(contactToClientView(queryItem, 'exist', communications));
		}
		this.helpersDialogVisible = true;
	}

	@Lock
	private async deleteHelper(event: MouseEvent) {
		event.stopPropagation();
		if (!this.selectedHelper || !this.selectedHelper[0]) {
			return;
		}
		if (!(await this.$confirm('Вы действительно хотите удалить этого помощника?'))) {
			return;
		}
		this.$store.dispatch(this.page.storeName + '/deleteHelper', this.selectedHelper[0]);
		this.selectedHelper = [];
	}

	@Lock
	private addContact(event: MouseEvent) {
		event.stopPropagation();
		this.contactFocused = {
			AccountId: this.page.entityId,
			ContactType: '',
			ContactValue: '',
			IsMain: false,
			$communications: [],
			Comments: '',
			SourceName: '',
			$state: 'new',
		};
		this.dlgContactEditMode = false;
		this.contactDialogVisible = true;
	}

	@Lock
	private addAddress(event: MouseEvent) {
		event.stopPropagation();
		this.addressFocused = {
			EntityId: this.page.entityId,
			EntityType: 'Account',
			$key: '',
			Description: '',
			IsPrimary: false,
			IsMailing: false,
			PostalCode: '',
			Country: '',
			State: '',
			County: '',
			City: '',
			Street: '',
			House: '',
			Office: '',
			FullAddress: '',
			SourceName: '',
			$state: 'new',
		};
		this.dlgAddressEditMode = false;
		this.addressDialogVisible = true;
	}

	@Lock
	private async deleteAddress(event: MouseEvent) {
		event.stopPropagation();
		if (!this.selectedAddress || !this.selectedAddress[0]) {
			return;
		}
		if (!(await this.$confirm('Вы действительно хотите удалить этот адрес?'))) {
			return;
		}
		this.$store.dispatch(this.page.storeName + '/deleteAddress', this.selectedAddress[0]);
		this.selectedHelper = [];
	}

	@Lock
	private async editAddress(event: MouseEvent) {
		event.stopPropagation();
		if (!this.selectedAddress || !this.selectedAddress[0] || !this.selectedAddress[0].$key) {
			return;
		}
		const queryList = await this.$api.select(
			`select ADDRESSID, ENTITYTYPE, DESCRIPTION, FULL_ADDRESS, ISPRIMARY, ISMAILING, POSTALCODE, COUNTRY, STATE, COUNTY, CITY, STREET, HOUSE, BUILDING, OFFICE, SOURCE_NAME from ADDRESS where ADDRESSID = '${this.selectedAddress[0].$key}'`,
			'$key, EntityType, Description, FullAddress, IsPrimary, IsMailing, PostalCode, Country, State, County, City, Street, House, Building, Office, SourceName',
		);
		if (!queryList || queryList.length < 1) return;
		const serverAddress = queryList[0];
		this.addressFocused = addressToClientView(serverAddress, 'exist');
		this.dlgAddressEditMode = true;
		this.addressDialogVisible = true;
	}

	@Lock
	private async editContact(event: MouseEvent) {
		event.stopPropagation();
		if (!this.selectedContact || !this.selectedContact[0] || !this.selectedContact[0].$key) return;
		const contactQuery = await this.$api.read('accContactInfoes', this.selectedContact[0].$key).then(x => x.json());
		const subqueryList = await this.$api.select(
			`select ACC_NEWSLETTERID, DELIVERY_TYPE, SUBSCRIPTION from ACC_NEWSLETTER where ACC_CONTACT_INFOID = '${contactQuery.$key}'`,
			'$key, DeliveryType, Subscription',
		);
		const communications: IAccNewsletter[] = [];
		if (subqueryList.length > 0) {
			for (const subqueryItem of subqueryList) communications.push(communicationToClientView(subqueryItem, 'exist'));
		}
		this.contactFocused = contactToClientView(contactQuery, 'exist', communications);
		this.dlgContactEditMode = true;
		this.contactDialogVisible = true;
	}

	@Lock
	private async deleteContact(event: MouseEvent) {
		event.stopPropagation();
		if (!this.selectedContact || !this.selectedContact[0]) return;
		if (!(await this.$confirm('Вы действительно хотите удалить этот контакт?'))) {
			return;
		}
		await this.$store.dispatch(this.page.storeName + '/deleteContact', this.selectedContact[0]);
		this.selectedContact = [];
	}

	@Lock
	private primaryHelperChanged(helperUpdated: IACCOUNTASSISTANT, isPrimary: boolean) {
		if (helperUpdated.ISMAIN == isPrimary) return;
		if (isPrimary) {
			for (const helper of this.helpers) {
				if (helper.ISMAIN) {
					if (helper.$state == 'exist' || !helper.$state) helper.$state = 'updated';
					helper.ISMAIN = false;
				}
			}
		}
		helperUpdated.ISMAIN = isPrimary;
		if (helperUpdated.$state == 'exist' || !helperUpdated.$state) helperUpdated.$state = 'updated';
	}

	@Lock
	private primaryContactChanged(contactUpdated: Contact, isPrimary: boolean) {
		if (contactUpdated.isPrimary == isPrimary) return;
		if (isPrimary) {
			if (contactUpdated.contactType && contactUpdated.contactType.includes('Тел')) {
				for (const contact of this.contacts) {
					if (contact.isPrimary && contact.contactType && contact.contactType.includes('Тел')) {
						if (contact.$state == 'exist' || !contact.$state) contact.$state = 'updated';
						contact.isPrimary = false;
					}
				}
			}
			if (
				contactUpdated.contactType &&
				(contactUpdated.contactType.toLowerCase().includes('e-mail') ||
					contactUpdated.contactType.toLowerCase().includes('email'))
			) {
				for (const contact of this.contacts) {
					if (
						contact.isPrimary &&
						(contactUpdated.contactType.toLowerCase().includes('e-mail') ||
							contactUpdated.contactType.toLowerCase().includes('email'))
					) {
						if (contact.$state == 'exist' || !contact.$state) contact.$state = 'updated';
						contact.isPrimary = false;
					}
				}
			}
		}
		contactUpdated.isPrimary = isPrimary;
		if (contactUpdated.$state == 'exist' || !contactUpdated.$state) contactUpdated.$state = 'updated';
	}

	@Lock
	private primaryAddressChanged(addressUpdated: Address, isPrimary: boolean) {
		if (addressUpdated.primary == isPrimary) return;
		if (isPrimary) {
			for (const address of this.addresses) {
				if (address.primary) {
					if (address.$state == 'exist' || !address.$state) address.$state = 'updated';
					address.primary = false;
				}
			}
		}
		addressUpdated.primary = isPrimary;
		if (addressUpdated.$state == 'exist' || !addressUpdated.$state) addressUpdated.$state = 'updated';
	}

	@Lock
	private mailingAddressChanged(addressUpdated: Address, isMailing: boolean) {
		if (addressUpdated.mailing == isMailing) return;
		if (isMailing) {
			for (const address of this.addresses) {
				if (address.mailing) {
					if (address.$state == 'exist' || !address.$state) address.$state = 'updated';
					address.mailing = false;
				}
			}
		}
		addressUpdated.mailing = isMailing;
		if (addressUpdated.$state == 'exist' || !addressUpdated.$state) addressUpdated.$state = 'updated';
	}

	@Lock
	private async helpersDialogClosed(isOK: boolean) {
		if (!this.helperFocused) return;
		if (isOK) {
			if (!this.helperFocused.FULLNAME) {
				this.$alert('Неверный формат поля "ФИО". Введите другие данные.');
				return;
			}
			if (this.helperFocused.$state == 'new') {
				await this.$store.dispatch(this.page.storeName + '/addHelper', {
					assistant: this.helperFocused,
					addresses: this.dlgHelpersAddresses,
					contacts: this.dlgHelpersContacts,
				});
				if (!this.dlgHelpers_itemId && this.helperFocused.COMPANYNAME) {
					await this.$api.service('AddPicklistItem', {
						Text: this.helperFocused.COMPANYNAME,
						ShortText: this.helperFocused.COMPANYNAME,
						PickListId: 'k6UJ9A0004YM',
						LanguageCode: 'ru-ru',
					});
				}
			} else if (this.helperFocused.$state == 'exist') {
				await this.$store.dispatch(this.page.storeName + '/updateHelper', {
					assistant: this.helperFocused,
					addresses: this.dlgHelpersAddresses,
					contacts: this.dlgHelpersContacts,
				});
				if (
					!this.dlgHelpers_itemId &&
					this.helperFocused.COMPANYNAME &&
					this.helperFocused.COMPANYNAME != this.helperFocused.$OLDCOMPANYNAME
				) {
					await this.$api.service('AddPicklistItem', {
						Text: this.helperFocused.COMPANYNAME,
						ShortText: this.helperFocused.COMPANYNAME,
						PickListId: 'k6UJ9A0004YM',
						LanguageCode: 'ru-ru',
					});
				}
			}
		}

		this.dlgHelpersAddresses = [];
		this.dlgHelpersContacts = [];
		this.helpersDialogVisible = false;
		this.dlgHelpersSelectedContact = [];
		this.dlgHelpersSelectedAddress = [];
	}

	@Lock
	private async contactDialogChange(contact: IAccContactInfo) {
		if (!contact) return;
		if (contact.AccountId) {
			if (this.dlgContactEditMode) {
				if (!(await this.$store.dispatch(this.page.storeName + '/updateContact', contact)))
					this.$alert('Ошибка при обновлении информации о контакте');
				this.selectedContact = [];
				this.dlgContactEditMode = false;
			} else await this.$store.dispatch(this.page.storeName + '/addContact', contact);
		} else {
			if (this.dlgContactEditMode) {
				if (this.dlgHelpersSelectedContact && this.dlgHelpersSelectedContact[0]) {
					const index = this.dlgHelpersContacts.indexOf(this.dlgHelpersSelectedContact[0]);
					if (index > -1) this.dlgHelpersContacts.splice(index, 1);
					this.dlgHelpersSelectedContact[0] = contact;
				}
				if (contact.$state == 'exist') contact.$state = 'updated';
				this.dlgContactEditMode = false;
			}
			if (contact.IsMain) {
				if (contact.ContactType && contact.ContactType.includes('Тел')) {
					for (const contact of this.dlgHelpersContacts)
						if (contact.IsMain && contact.ContactType && contact.ContactType.includes('Тел')) {
							if (contact.$state == 'exist') contact.$state = 'updated';
							contact.IsMain = false;
						}
				}
				if (
					contact.ContactType &&
					(contact.ContactType.toLowerCase().includes('e-mail') || contact.ContactType.toLowerCase().includes('email'))
				) {
					for (const contact of this.dlgHelpersContacts)
						if (
							contact.IsMain &&
							contact.ContactType &&
							(contact.ContactType.toLowerCase().includes('e-mail') ||
								contact.ContactType.toLowerCase().includes('email'))
						) {
							if (contact.$state == 'exist') contact.$state = 'updated';
							contact.IsMain = false;
						}
				}
			}
			this.dlgHelpersContacts.push(contact);
		}
	}

	contactDialogClosed() {
		this.contactDialogVisible = false;
	}

	@Lock
	private async addressDialogClosed(isOK: boolean) {
		if (isOK && this.addressFocused) {
			if (this.addressFocused.EntityType == 'Account') await this.addressDialogClosedOnAccountsTab();
			else if (this.addressFocused.EntityType == 'AccountAssistant') await this.addressDialogClosedInHelpersDialog();
			else {
				this.$alert('Неопределенная принадлежность адреса. Скопируйте данные и создайте новый адрес.');
			}
		} else this.addressDialogVisible = false;
	}

	private async addressDialogClosedOnAccountsTab() {
		if (!this.addressFocused) return;
		if (!this.addressFocused.Description || this.addressFocused.Description == '') {
			this.$alert('Выберите тип адреса.');
			return;
		}
		if (!this.addressFocused.FullAddress || this.addressFocused.FullAddress == '') {
			this.$alert('Заполните поле "Адрес одной строкой".');
			return;
		}
		if (this.dlgAddressEditMode) {
			if (!(await this.$confirm('Вы действительно хотите изменить информацию об адресе?'))) return;
			if (!(await this.$store.dispatch(this.page.storeName + '/updateAddress', this.addressFocused)))
				// actions вызываюся через dispatch
				this.$alert('Ошибка при обновлении информации об адресе'); // mutations вызываются через commit
			if (this.addressFocused.IsMailing)
				this.$store.commit(this.page.storeName + '/changeAddress', this.addressFocused);
			this.selectedAddress = [];
			this.dlgAddressEditMode = false;
		} else await this.$store.dispatch(this.page.storeName + '/addAddress', this.addressFocused);
		this.addressDialogVisible = false;
	}

	private async addressDialogClosedInHelpersDialog() {
		if (!this.addressFocused) return;
		if (!this.addressFocused.Description || this.addressFocused.Description == '') {
			this.$alert('Выберите тип адреса.');
			return;
		}
		if (!this.addressFocused.FullAddress || this.addressFocused.FullAddress == '') {
			this.$alert('Заполните поле "Адрес одной строкой".');
			return;
		}
		if (
			!this.dlgAddressEditMode &&
			this.dlgHelpersAddresses.find(address => {
				return address.FullAddress == this.addressFocused!.FullAddress;
			})
		) {
			this.$alert('Такой адрес уже существует. Введите другие данные.');
			return;
		}
		if (this.dlgAddressEditMode) {
			if (!(await this.$confirm('Вы действительно хотите изменить информацию об адресе?'))) return;
			if (this.dlgHelpersSelectedAddress && this.dlgHelpersSelectedAddress[0]) {
				const index = this.dlgHelpersAddresses.indexOf(this.dlgHelpersSelectedAddress[0]);
				if (index > -1) this.dlgHelpersAddresses.splice(index, 1);
				this.dlgHelpersSelectedAddress[0] = this.addressFocused;
			}
			if (this.addressFocused.$state == 'exist') this.addressFocused.$state = 'updated';
			this.dlgAddressEditMode = false;
		}
		if (this.addressFocused.IsPrimary) {
			for (const address of this.dlgHelpersAddresses) {
				if (address.IsPrimary) {
					if (address.$state == 'exist') address.$state = 'updated';
					address.IsPrimary = false;
				}
			}
		}
		this.dlgHelpersAddresses.push(this.addressFocused);
		this.addressDialogVisible = false;
	}

	@Lock
	private dlgHelpersAddContact(event: MouseEvent) {
		this.contactFocused = {
			ContactType: '',
			ContactValue: '',
			IsMain: false,
			$communications: [],
			Comments: '',
			SourceName: '',
			$state: 'new',
		};
		this.dlgContactEditMode = false;
		this.contactDialogVisible = true;
		event.stopPropagation();
	}

	@Lock
	private dlgHelpersEditContact(event: MouseEvent) {
		event.stopPropagation();
		if (!this.dlgHelpersSelectedContact || !this.dlgHelpersSelectedContact[0]) return;
		this.contactFocused = JSON.parse(JSON.stringify(this.dlgHelpersSelectedContact[0]));
		this.contactDialogVisible = true;
		this.dlgContactEditMode = true;
	}

	@Lock
	private async dlgHelpersDeleteContact(event: MouseEvent) {
		event.stopPropagation();
		if (!this.contactFocused || !this.dlgHelpersSelectedContact || !this.dlgHelpersSelectedContact[0]) {
			return;
		}
		if (!(await this.$confirm('Вы действительно хотите удалить этот контакт?'))) {
			return;
		}
		const index = this.dlgHelpersContacts.indexOf(this.dlgHelpersSelectedContact[0]);
		if (index > -1) this.dlgHelpersContacts.splice(index, 1);
		if (this.dlgHelpersSelectedContact[0].$state == 'exist' || this.dlgHelpersSelectedContact[0].$state == 'updated') {
			if (
				this.dlgHelpersSelectedContact[0].$communications &&
				this.dlgHelpersSelectedContact[0].$communications.length > 0
			) {
				for (const communication of this.dlgHelpersSelectedContact[0].$communications) {
					if (communication.$state == 'exist' || communication.$state == 'updated') {
						if (communication.$key) await this.$api.delete('accNewsletters', communication.$key);
					}
				}
			}
			if (this.dlgHelpersSelectedContact[0].$key)
				await this.$api.delete('accContactInfoes', this.dlgHelpersSelectedContact[0].$key);
		}
		this.dlgHelpersSelectedContact = [];
	}

	@Lock
	private dlgHelpersAddAddress(event: MouseEvent) {
		event.stopPropagation();
		this.addressFocused = {
			$key: '',
			EntityType: 'AccountAssistant',
			Description: '',
			IsPrimary: false,
			IsMailing: false,
			PostalCode: '',
			Country: '',
			State: '',
			County: '',
			City: '',
			Street: '',
			House: '',
			Office: '',
			FullAddress: '',
			SourceName: '',
			$state: 'new',
		};
		this.addressDialogVisible = true;
	}

	@Lock
	private dlgHelpersEditAddress(event: MouseEvent) {
		event.stopPropagation();
		if (!this.dlgHelpersSelectedAddress || !this.dlgHelpersSelectedAddress[0]) return;
		this.addressFocused = JSON.parse(JSON.stringify(this.dlgHelpersSelectedAddress[0]));
		this.addressDialogVisible = true;
		this.dlgAddressEditMode = true;
	}

	@Lock
	private async dlgHelpersDeleteAddress(event: MouseEvent) {
		event.stopPropagation();
		if (!this.dlgHelpersSelectedAddress || !this.dlgHelpersSelectedAddress[0]) return;
		if (!(await this.$confirm('Вы действительно хотите удалить этот адрес?'))) {
			return;
		}
		const index = this.dlgHelpersAddresses.indexOf(this.dlgHelpersSelectedAddress[0]);
		if (index > -1) this.dlgHelpersAddresses.splice(index, 1);
		if (this.dlgHelpersSelectedAddress[0].$state == 'exist' || this.dlgHelpersSelectedAddress[0].$state == 'updated') {
			if (this.dlgHelpersSelectedAddress[0].$key) this.$api.delete('addresses', this.dlgHelpersSelectedAddress[0].$key);
		}
		this.dlgHelpersSelectedAddress = [];
	}

	@Lock
	private dlgHelpersMainContactChanged(contactUpdated: IAccContactInfo, isMain: boolean) {
		if (isMain) {
			if (contactUpdated.ContactType && contactUpdated.ContactType.includes('Тел')) {
				for (const contact of this.dlgHelpersContacts) {
					if (contact.IsMain && contact.ContactType && contact.ContactType.includes('Тел')) {
						if (contact.$state == 'exist') contact.$state = 'updated';
						contact.IsMain = false;
					}
				}
			}
			if (
				contactUpdated.ContactType &&
				(contactUpdated.ContactType.toLowerCase().includes('e-mail') ||
					contactUpdated.ContactType.toLowerCase().includes('email'))
			) {
				for (const contact of this.dlgHelpersContacts) {
					if (
						contact.IsMain &&
						(contactUpdated.ContactType.toLowerCase().includes('e-mail') ||
							contactUpdated.ContactType.toLowerCase().includes('email'))
					) {
						if (contact.$state == 'exist') contact.$state = 'updated';
						contact.IsMain = false;
					}
				}
			}
		}
		if (contactUpdated.$state == 'exist') contactUpdated.$state = 'updated';
		contactUpdated.IsMain = isMain;
	}

	@Lock
	private dlgHelpersPrimaryAddressChanged(addressUpdated: IAddress, isPrimary: boolean) {
		if (addressUpdated.IsPrimary == isPrimary) return;
		if (isPrimary) {
			for (const address of this.dlgHelpersAddresses) {
				if (address.IsPrimary) {
					if (address.$state == 'exist') address.$state = 'updated';
					address.IsPrimary = false;
				}
			}
		}
		addressUpdated.IsPrimary = isPrimary;
		if (addressUpdated.$state == 'exist') addressUpdated.$state = 'updated';
	}

	@Lock
	private dlgHelpersMailingAddressChanged(addressUpdated: IAddress, isMailing: boolean) {
		if (addressUpdated.IsMailing == isMailing) return;
		if (isMailing) {
			for (const address of this.dlgHelpersAddresses) {
				if (address.IsMailing) {
					if (address.$state == 'exist') address.$state = 'updated';
					address.IsMailing = false;
				}
			}
		}
		addressUpdated.IsMailing = isMailing;
		if (addressUpdated.$state == 'exist') addressUpdated.$state = 'updated';
	}

	private getHelperPrimaryPhoneText(helper: IACCOUNTASSISTANT): string {
		if (helper.$primaryPhone && helper.$primaryPhone.ContactType && helper.$primaryPhone.ContactValue)
			return helper.$primaryPhone.ContactType + ': ' + helper.$primaryPhone.ContactValue;
		return '';
	}

	private getHelperPrimaryEmailText(helper: IACCOUNTASSISTANT): string {
		if (helper.$primaryEmail && helper.$primaryEmail.ContactType && helper.$primaryEmail.ContactValue)
			return helper.$primaryEmail.ContactType + ': ' + helper.$primaryEmail.ContactValue;
		return '';
	}

	private getHelperPrimaryAddressText(helper: IACCOUNTASSISTANT): string {
		if (helper.$primaryAddress && helper.$primaryAddress.Description && helper.$primaryAddress.FullAddress)
			return helper.$primaryAddress.Description + ': ' + helper.$primaryAddress.FullAddress;
		return '';
	}

	private helperPrimaryPhoneLnkClick(helper: IACCOUNTASSISTANT) {
		this.$alert('Временно отключено');
	}

	private helperPrimaryEmailLnkClick(helper: IACCOUNTASSISTANT) {
		if (helper.$primaryEmail) this.page.showDialog('v-dialog-ao-email', helper.$primaryEmail);
	}

	private helperPrimaryAddressLnkClick(helper: IACCOUNTASSISTANT) {
		this.$alert('Временно отключено');
	}
}
</script>

<style lang="scss">
.v-tab-contacts {
	padding-top: 4px;

	&-mainarea {
		padding: 8px 4px;
		display: grid;
		grid-template-areas: 'label checkbox';
		grid-template-columns: 230px 25px;
		grid-column-gap: $distance-x2s;

		.v-label {
			grid-area: label;
			color: red;
			font-weight: bold;
		}

		.v-checkbox {
			grid-area: checkbox;
		}
	}

	.contactsGrid {
		.v-data-table__table {
			margin-top: 8px;
		}
	}
}

.dialog-helpers-main {
	display: grid;
	grid-template-areas: 'left right';
	grid-template-columns: 1fr 1fr;
	grid-column-gap: $distance-lg;

	&-left {
		grid-area: left;
		display: grid;
		grid-template-areas:
			'fullname'
			'organization'
			'department'
			'title';
		grid-template-rows: 1fr 1fr 1fr 1fr;
		grid-row-gap: $distance-x2s;

		&-fullname {
			grid-area: fullname;
			display: grid;
			grid-template-areas: 'label text';
			grid-template-columns: 160px 320px;
			grid-column-gap: $distance-xs;

			&-label {
				grid-area: label;
				padding-top: 4px;
			}

			&-input {
				grid-area: text;
			}
		}

		&-organization {
			grid-area: organization;
			display: grid;
			grid-template-areas: 'label text';
			grid-template-columns: 160px 320px;
			grid-column-gap: $distance-xs;

			&-label {
				grid-area: label;
				padding-top: 4px;
			}

			&-input {
				grid-area: text;
			}
		}

		&-department {
			grid-area: department;
			display: grid;
			grid-template-areas: 'label text';
			grid-template-columns: 160px 320px;
			grid-column-gap: $distance-xs;

			&-label {
				grid-area: label;
				padding-top: 4px;
			}

			&-input {
				grid-area: text;
			}
		}

		&-title {
			grid-area: title;
			display: grid;
			grid-template-areas: 'label text';
			grid-template-columns: 160px 320px;
			grid-column-gap: $distance-xs;

			&-label {
				grid-area: label;
				padding-top: 4px;
			}

			&-input {
				grid-area: text;
			}
		}
	}

	&-right {
		grid-area: right;
		display: grid;
		grid-template-areas:
			'isPrimary'
			'comment';
		grid-template-rows: 20px 93px;
		grid-row-gap: 16px;
		padding-top: 4px;

		&-isPrimary {
			grid-area: isPrimary;
			display: grid;
			grid-template-areas: 'label chk';
			grid-template-columns: 120px 24px;
			grid-column-gap: $distance-xs;

			&-label {
				grid-area: label;
			}

			&-chk {
				grid-area: chk;
			}
		}

		&-comment {
			grid-area: comment;
			display: grid;
			grid-template-areas:
				'label'
				'text';
			grid-template-rows: 20px 1fr;
			grid-row-gap: 10px;

			&-label {
				grid-area: label;
			}

			&-text {
				grid-area: text;
				width: minmax(150px, 488px);
			}
		}
	}
}

.dialog-helpers-contacts1 {
	padding-top: 16px;

	.v-data-table__table {
		margin-top: 8px;
	}
}

.dialog-helpers-contacts2 {
	padding-top: 16px;

	.v-data-table__table {
		margin-top: 8px;
	}
}

.dialog-contact {
	display: grid;
	grid-template-areas:
		'type'
		'value'
		'isPrimary'
		'commentary'
		'source'
		'communications';
	grid-template-rows: 32px 32px 32px 64px 32px auto;
	grid-row-gap: $distance-xs;

	&-type {
		grid-area: type;
		display: grid;
		grid-template-areas: 'lbl pkl';
		grid-template-columns: 1fr 1fr;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
		}

		&-pkl {
			grid-area: pkl;
		}
	}

	&-value {
		grid-area: value;
		display: grid;
		grid-template-areas: 'lbl txt';
		grid-template-columns: 1fr 1fr;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
		}

		&-txt {
			grid-area: txt;
		}
	}

	&-primary {
		grid-area: isPrimary;
		display: grid;
		grid-template-areas: 'lbl chk';
		grid-template-columns: 1fr 1fr;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
		}

		&-chk {
			grid-area: chk;
		}
	}

	&-communications {
		grid-area: communications;

		.v-data-table__table {
			margin-top: 8px;
		}
	}

	&-commentary {
		grid-area: commentary;
		display: grid;
		grid-template-areas: 'lbl txt';
		grid-template-columns: 1fr 1fr;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
		}

		&-txt {
			grid-area: txt;
		}
	}

	&-source {
		grid-area: source;
		display: grid;
		grid-template-areas: 'lbl txt';
		grid-template-columns: 1fr 1fr;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
		}

		&-txt {
			grid-area: txt;
		}
	}
}

.dialog-address {
	display: grid;
	grid-template-areas:
		'type'
		'primary'
		'mailing'
		'index'
		'country'
		'subject'
		'region'
		'city'
		'street'
		'house'
		'building'
		'apartment'
		'fulladdress'
		'source';
	grid-template-rows: 64px 32px 32px 32px 32px 32px 32px 32px 32px 32px 32px 32px 64px 32px;
	grid-row-gap: $distance-xs;

	&-type {
		grid-area: type;
		display: grid;
		grid-template-areas: 'lbl pkl';
		grid-template-columns: 115px 340px;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
			padding-top: 4px;
		}

		&-pkl {
			grid-area: pkl;
		}
	}

	&-primary {
		grid-area: primary;
		display: grid;
		grid-template-areas: 'lbl chk';
		grid-template-columns: 115px 340px;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
		}

		&-chk {
			grid-area: chk;
		}
	}

	&-mailing {
		grid-area: mailing;
		display: grid;
		grid-template-areas: 'lbl check';
		grid-template-columns: 115px 340px;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
		}

		&-check {
			grid-area: check;
		}
	}

	&-index {
		grid-area: index;
		display: grid;
		grid-template-areas: 'lbl txt';
		grid-template-columns: 115px 340px;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
			padding-top: 4px;
		}

		&-txt {
			grid-area: txt;
		}
	}

	&-country {
		grid-area: country;
		display: grid;
		grid-template-areas: 'lbl txt';
		grid-template-columns: 115px 340px;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
			padding-top: 4px;
		}

		&-txt {
			grid-area: txt;
		}
	}

	&-subject {
		grid-area: subject;
		display: grid;
		grid-template-areas: 'lbl txt';
		grid-template-columns: 115px 340px;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
			padding-top: 4px;
		}

		&-txt {
			grid-area: txt;
		}
	}

	&-region {
		grid-area: region;
		display: grid;
		grid-template-areas: 'lbl txt';
		grid-template-columns: 115px 340px;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
			padding-top: 4px;
		}

		&-txt {
			grid-area: txt;
		}
	}

	&-city {
		grid-area: city;
		display: grid;
		grid-template-areas: 'lbl txt';
		grid-template-columns: 115px 340px;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
			padding-top: 4px;
		}

		&-txt {
			grid-area: txt;
		}
	}

	&-street {
		grid-area: street;
		display: grid;
		grid-template-areas: 'lbl txt';
		grid-template-columns: 115px 340px;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
			padding-top: 4px;
		}

		&-txt {
			grid-area: txt;
		}
	}

	&-house {
		grid-area: house;
		display: grid;
		grid-template-areas: 'lbl txt';
		grid-template-columns: 115px 340px;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
			padding-top: 4px;
		}

		&-txt {
			grid-area: txt;
		}
	}

	&-building {
		grid-area: building;
		display: grid;
		grid-template-areas: 'lbl txt';
		grid-template-columns: 115px 340px;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
			padding-top: 4px;
		}

		&-txt {
			grid-area: txt;
		}
	}

	&-apartment {
		grid-area: apartment;
		display: grid;
		grid-template-areas: 'lbl txt';
		grid-template-columns: 115px 340px;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
			padding-top: 4px;
		}

		&-txt {
			grid-area: txt;
		}
	}

	&-fulladdress {
		grid-area: fulladdress;
		display: grid;
		grid-template-areas: 'lbl txt';
		grid-template-columns: 115px 340px;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
		}

		&-txt {
			grid-area: txt;
		}
	}

	&-source {
		grid-area: source;
		display: grid;
		grid-template-areas: 'lbl txt';
		grid-template-columns: 115px 340px;
		grid-column-gap: $distance-xs;

		&-lbl {
			grid-area: lbl;
			padding-top: 4px;
		}

		&-txt {
			grid-area: txt;
		}
	}
}
</style>