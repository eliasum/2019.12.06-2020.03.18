<template>
	<v-modal caption="Активные операции (телефон)" :visible="visible">
		<div class="v-dialog-ao-phone">
			<div class="v-dialog-ao-phone-top">
				<div class="v-dialog-ao-phone-top-lblType">
					<v-label :text="'Тип контакта:'" element="txt-contactType" />
				</div>
				<div class="v-dialog-ao-phone-top-txtType">
					<v-text-input id="txt-contactType" narrow noLabel readonly :value="contact.ContactType" />
				</div>
				<div class="v-dialog-ao-phone-top-lblValue">
					<v-label :text="'Контакт:'" element="txt-contactValue" />
				</div>
				<div class="v-dialog-ao-phone-top-txtValue">
					<v-text-input id="txt-contactValue" narrow noLabel readonly :value="contact.ContactValue" />
				</div>
				<div class="v-dialog-ao-phone-top-lblComment">
					<v-label :text="'Комментарии к контакту:'" element="txt-comments" />
				</div>
				<div class="v-dialog-ao-phone-top-txtComment">
					<v-textarea id="txt-comments" noLabel :rows="3" v-model="contact.Comments" />
				</div>
			</div>
			<hr />
			<div class="v-dialog-ao-phone-middle">
				<div class="v-dialog-ao-phone-middle-lblCalls">
					<v-label :text="'Запланированные и/или просроченные звонки:'" element="txt-calls" />
				</div>
				<div class="v-dialog-ao-phone-middle-txtCalls">
					<v-link :active="false" :text="activities.length.toString()" />
				</div>
			</div>
			<v-data-table :data="activities" idField="$key" :columns="activitiesColumns" renderMode="all" narrow :height="250" allowDetail>
				<template #default="{entity}">
					<td class="v-data-table-td--0">{{entity.StartDate | formatLocal}}</td>
					<td class="v-data-table-td--1">
						<v-link text="Звонок" @click="editCall(entity)" />
					</td>
					<td class="v-data-table-td--2">{{entity.Description}}</td>
					<td class="v-data-table-td--3">{{entity.Status}}</td>
					<td class="v-data-table-td--3">{{entity.$UserName}}</td>
					<td class="v-data-table-td--4">
						<v-link text="Завершить" @click="endCall(entity)" />
					</td>
				</template>
				<template #details="{entity}">
					<div class="v-dialog-ao-phone-commentary">
						<div class="v-dialog-ao-phone-commentary-lblNotes">
							<v-label :text="'Комментарий:'" />
						</div>
						<div class="v-dialog-ao-phone-commentary-txtNotes">{{entity.LongNotes}}</div>
					</div>
				</template>
			</v-data-table>
		</div>
		<template #footer>
			<div>
				<v-button flat regular @click="call()">{{'Вызов'}}</v-button>
				<v-button flat regular @click="createCall()">{{'Запланировать звонок'}}</v-button>
				<v-button flat regular @click="save()">{{'Сохранить'}}</v-button>
				<v-button flat regular @click="cancel()">{{'Отмена'}}</v-button>
			</div>
		</template>
	</v-modal>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import PageBase from '@/base-components/abstract/v-page-base';
import Getter from '@/decorators/Getter';
import { IAccContactInfo } from '@/model/interfaces/IAccContactInfo';
import { IActivity } from '@/model/interfaces/IActivity';

@Component
export default class DialogActiveOperationsPhone extends Vue {
	@Getter('page') public readonly page!: PageBase;
	@Prop({ type: Boolean, required: true }) readonly visible!: boolean;
	@Prop({ required: true }) readonly contact!: IAccContactInfo;
	@Prop({ type: Array, required: true }) readonly activities!: IActivity[];

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

	private get activitiesColumns() {
		return [
			{ fieldName: 'StartDate', caption: 'Дата/время' },
			{ fieldName: 'Type', caption: 'Тип' },
			{ fieldName: 'Description', caption: 'Тема' },
			{ fieldName: 'Status', caption: 'Статус' },
			{ fieldName: '$UserName', caption: 'Пользователь' },
			{ fieldName: '$key', caption: 'Завершить' },
		];
	}

	editCall(payload: IActivity) {
		window.Sage.Link.editActivity(payload.$key, false);
		this.$emit('close');
	}

	endCall(payload: IActivity) {
		window.Sage.Link.completeActivity(payload.$key, false);
		this.$emit('close');
	}

	call() {
		window.Sage.Link.completeNewActivity(262146, false);
		this.$emit('close');
	}

	createCall() {
		window.Sage.Link.schedulePhoneCall();
		this.$emit('close');
	}

	async save() {
		this.$emit('change', this.contact);

		this.$emit('close');

		await this.$store.dispatch(this.storeName + '/load', this.entityId); // перезагрузка страницы
	}

	cancel() {
		this.$emit('close');
	}
}
</script>

<style lang="scss">
.v-dialog-ao-phone-top {
	display: grid;
	grid-template-areas:
		'lblType txtType txtType txtType'
		'lblValue txtValue txtValue txtValue'
		'lblComment lblComment . .'
		'txtComment txtComment txtComment txtComment';
	grid-template-columns: 150px minmax(50px, 1fr) minmax(50px, 1fr) minmax(50px, 1fr);
	grid-gap: $distance-xs;

	&-lblType {
		grid-area: lblType;
		width: 75px;
		padding-top: 4px;
	}

	&-txtType {
		grid-area: txtType;
	}

	&-lblValue {
		grid-area: lblValue;
		padding-top: 4px;
	}

	&-txtValue {
		grid-area: txtValue;
	}

	&-lblComment {
		grid-area: lblComment;
	}

	&-txtComment {
		grid-area: txtComment;
	}
}

.v-dialog-ao-phone-middle {
	display: grid;
	grid-template-areas: 'lblCalls txtCalls';
	grid-template-columns: 275px 35px;

	&-lblCalls {
		grid-area: lblCalls;
	}

	&-txtCalls {
		grid-area: txtCalls;
	}
}

.v-dialog-ao-phone-commentary {
	display: grid;
	grid-template-areas: 'lblNotes txtNotes';
	grid-template-columns: 143px minmax(143px, 1fr);

	&-lblNotes {
		grid-area: lblNotes;
	}

	&-txtNotes {
		grid-area: txtNotes;
	}
}
</style>