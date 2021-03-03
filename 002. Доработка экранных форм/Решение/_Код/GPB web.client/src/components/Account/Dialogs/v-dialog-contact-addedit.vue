<template>
	<div>
		<v-lookup
			@change="communicationLookupValueSelected($event)"
			caption="Поиск типа коммуникации"
			select="select PICKLISTITEMID, TEXT from PICKLISTITEMVIEW where PICKLISTID = 'k6UJ9A0004XA'"
			idField="$key"
			style="display:none"
			:lookupVisible="communicationLookupVisible"
			@closing="communicationLookupClosing()"
			:columns="communicationLookupColumns"
			noFilter
			:hiddenFilters="communicationLookupFilters"
			init
			:zindex="952"
		/>
		<v-modal caption="Добавить/Изменить контакт" showOk :visible="visible" @close="contactDialogClosed($event)" :width="500" :height="540">
			<div class="dialog-contact">
				<div class="dialog-contact-type">
					<div class="dialog-contact-type-lbl">
						<v-label :text="'Тип контакта:'" element="dlgContact-pkl_Type" />
					</div>
					<div class="dialog-contact-type-pkl">
						<v-picklist id="dlgHelpers-pkl_Type" bordered narrow noLabel picklistName="Тип контакта" v-model="contact.ContactType" searchable />
					</div>
				</div>
				<div class="dialog-contact-value">
					<div class="dialog-contact-value-lbl">
						<v-label :text="'Контакт:'" element="dlgContact-txt_Value" />
					</div>
					<div class="dialog-contact-value-txt">
						<v-regexp-text-input
							id="dlgContact-txt_Value"
							bordered
							narrow
							noLabel
							v-model="contact.ContactValue"
							:readonly="!contact.ContactType"
							:validation="contactValueValidation"
							:mask="getContactValueMask"
						/>
					</div>
				</div>
				<div class="dialog-contact-primary">
					<div class="dialog-contact-primary-lbl">
						<v-label :text="'Основной контакт:'" element="dlgContact-chk_Primary" />
					</div>
					<div class="dialog-contact-primary-chk">
						<v-checkbox id="dlgHelpers-chk_IsPrimary" noLabel size="small" v-model="contact.IsMain" />
					</div>
				</div>
				<div class="dialog-contact-communications">
					<v-accordion caption="Коммуникации" :active="false">
						<template v-slot:buttons>
							<v-tooltip direction="top">
								<v-button circle flat class="v-accordion-buttonContainer-button" @click="addCommunication($event)">{{'&oplus;'}}</v-button>
								<template #tooltip>{{'Добавить'}}</template>
							</v-tooltip>
							<v-tooltip direction="top">
								<v-button circle flat class="v-accordion-buttonContainer-button" @click="deleteCommunication($event)">{{'&#8722;'}}</v-button>
								<template #tooltip>{{'Удалить'}}</template>
							</v-tooltip>
						</template>
						<v-data-table
							ref="communicationsTable"
							:data="contact.$communications"
							idField="$key"
							:columns="communicationColumns"
							renderMode="pagination"
							narrow
							:height="140"
							selectionMode="single"
							v-model="selectedCommunication"
						>
							<template #default="{entity}">
								<td class="v-data-table-td--0">{{entity.DeliveryType}}</td>
								<td class="v-data-table-td--1">
									<v-select
										:value="entity.Subscription"
										@change="changeCommunicationStatus(entity, $event)"
										narrow
										noLabel
										:options="communicationStatusOptions"
										:zindex="952"
									/>
								</td>
							</template>
						</v-data-table>
					</v-accordion>
				</div>
				<div class="dialog-contact-commentary">
					<div class="dialog-contact-commentary-lbl">
						<v-label :text="'Комментарий:'" element="dlgContact-txt_Commentary" />
					</div>
					<div class="dialog-contact-commentary-txt">
						<v-textarea id="dlgContact-txt_Commentary" noLabel :rows="3" v-model="contact.Comments" />
					</div>
				</div>
				<div class="dialog-contact-source">
					<div class="dialog-contact-source-lbl">
						<v-label :text="'Источник:'" element="dlgContact-txt_Source" />
					</div>
					<div class="dialog-contact-source-txt">
						<v-text-input id="dlgContact-txt_Source" bordered narrow noLabel v-model="contact.SourceName" readonly />
					</div>
				</div>
			</div>
		</v-modal>
	</div>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import PageBase from '@/base-components/abstract/v-page-base';
import Getter from '@/decorators/Getter';
import { IAccContactInfo } from '@/model/interfaces/IAccContactInfo';
import Lock from '@/decorators/Lock';
import LockQueue from '@/decorators/LockQueue';
import { IAccNewsletter, toClientViewFromSelect as communicationToClientView } from '@/model/interfaces/IAccNewsletter';
import DataTableComponent from '@/base-components/v-data-table.vue';

@Component
export default class DialogContactAddEdit extends Vue {
	@Getter('page') public readonly page!: PageBase;
	@Prop({ type: Boolean, required: true }) readonly visible!: boolean;
	@Prop({ required: true }) readonly contact!: IAccContactInfo;

	$refs!: {
		communicationsTable: DataTableComponent;
	};

	public get storeName(): string {
		return window.Sage && window.Sage.Data && window.Sage.Data.EntityContextStore
			? window.Sage.Data.EntityContextStore.EntityType.substring(24)
			: '';
	}

	public get entityId(): string {
		return window.Sage && window.Sage.Data && window.Sage.Data.EntityContextStore
			? window.Sage.Data.EntityContextStore.EntityId
			: '';
	}

	private communicationLookupVisible = false;

	private selectedCommunication: IAccNewsletter[] | null = null;

	@Lock
	private async contactDialogClosed(isOK: boolean) {
		if (!this.contact) return;
		if (isOK) {
			if (!this.contact.ContactValue || !this.contactValueValidation(this.contact.ContactValue)) {
				this.$alert('Неверный формат поля "Контакт". Введите другие данные.');
				return;
			}

			if (this.contact.$key && !(await this.$confirm('Вы действительно хотите изменить информацию о контакте?')))
				return;
			this.$emit('change', this.contact);
		}
		this.$emit('close');

		await this.$store.dispatch(this.storeName + '/load', this.entityId); // перезагрузка страницы
	}

	private contactValueValidation(payload: string): boolean {
		const regex: RegExp | null = this.getContactValueRegExp;
		if (regex) return regex.test(payload.toLowerCase());
		else return true;
	}

	private get getContactValueRegExp(): RegExp | null {
		if (!this.contact || !this.contact.ContactType) return null;
		if (this.contact.ContactType.toLowerCase().includes('тел')) {
			return /^(\s*)?(\+)?([- _():=+]?\d[- _():=+]?){10,14}(\s*)?$/;
		}
		if (
			this.contact.ContactType.toLowerCase().includes('e-mail') ||
			this.contact.ContactType.toLowerCase().includes('email')
		) {
			// eslint-disable-next-line no-useless-escape
			return /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
		}
		return null;
	}

	private get getContactValueMask(): string {
		if (!this.contact || !this.contact.ContactType) return '';
		if (this.contact.ContactType.toLowerCase().includes('тел')) {
			return '+#-###-###-##-##';
		}
		if (
			this.contact.ContactType.toLowerCase().includes('e-mail') ||
			this.contact.ContactType.toLowerCase().includes('email')
		) {
			return '';
		}
		return '';
	}

	private communicationLookupClosing() {
		this.communicationLookupVisible = false;
	}

	@Lock
	private addCommunication(event: MouseEvent) {
		this.communicationLookupVisible = true;
		event.stopPropagation();
	}

	@LockQueue(true)
	private communicationLookupValueSelected(payload: IAccNewsletter) {
		if (this.contact && this.contact.$communications) {
			payload.Subscription = 'Пусто';
			payload.$state = 'new';
			this.contact.$communications.push(payload);
		}
	}

	private get communicationLookupFilters() {
		const filters: any[] = [];
		if (!this.contact || !this.contact.$communications) return filters;
		for (const communication of this.contact.$communications)
			filters.push({ fieldName: 'DeliveryType', condition: '!=', value: communication.DeliveryType });

		return filters;
	}

	private get communicationStatusOptions() {
		return ['Согласие', 'Отказ', 'Пусто'];
	}

	@Lock
	private changeCommunicationStatus(entity: IAccNewsletter, payload: string) {
		if (entity.Subscription != payload) {
			entity.Subscription = payload;
			if (entity.$state == 'exist') entity.$state = 'updated';
			// !Не повторять. Переделать привязки на корректные
			this.$refs.communicationsTable.$forceUpdate();
		}
	}

	@Lock
	private async deleteCommunication(event: MouseEvent) {
		event.stopPropagation();
		if (
			!this.contact ||
			!this.contact.$communications ||
			!this.selectedCommunication ||
			!this.selectedCommunication[0]
		) {
			return;
		}
		if (!(await this.$confirm('Вы действительно хотите удалить эту коммуникацию?'))) {
			return;
		}
		const index = this.contact.$communications.indexOf(this.selectedCommunication[0]);
		if (index > -1) this.contact.$communications.splice(index, 1);
		if (this.selectedCommunication[0].$state == 'exist' || this.selectedCommunication[0].$state == 'updated') {
			if (this.selectedCommunication[0].$key) this.$api.delete('accNewsletters', this.selectedCommunication[0].$key);
		}
		this.selectedCommunication = [];
	}

	private get communicationColumns() {
		return [
			{ fieldName: 'DeliveryType', caption: 'Тип коммуникации' },
			{ fieldName: 'Subscription', caption: 'Статус' },
		];
	}

	private get communicationLookupColumns() {
		return [
			{ fieldName: '$key', visible: false, sqlFieldName: 'PICKLISTITEMID' },
			{ fieldName: 'DeliveryType', caption: 'Тип коммуникации', isDisplayName: true, sqlFieldName: 'TEXT' },
		];
	}
}
</script>

<style lang="scss">
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
</style>