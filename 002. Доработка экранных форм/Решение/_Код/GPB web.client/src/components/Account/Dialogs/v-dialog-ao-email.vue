<template>
	<v-modal caption="Активные операции (e-mail)" :visible="visible" :width="600" @close="$emit('close', $event)">
		<div class="v-dialog-ao-email">
			<div class="v-dialog-ao-email-lblType">
				<v-label :text="'Тип контакта:'" element="txt-contactType" />
			</div>
			<div class="v-dialog-ao-email-txtType">
				<v-text-input id="txt-contactType" narrow noLabel readonly :value="contact.ContactType" />
			</div>
			<div class="v-dialog-ao-email-lblValue">
				<v-label :text="'Контакт:'" element="txt-contactValue" />
			</div>
			<div class="v-dialog-ao-email-txtValue">
				<v-text-input id="txt-contactValue" narrow noLabel readonly :value="contact.ContactValue" />
			</div>
			<div class="v-dialog-ao-email-lblComment">
				<v-label :text="'Комментарии к контакту:'" element="txt-comments" />
			</div>
			<div class="v-dialog-ao-email-txtComment">
				<v-textarea id="txt-comments" noLabel :rows="3" v-model="mutableComments" />
			</div>
		</div>
		<template #footer>
			<div>
				<v-button flat regular @click="createEmail()">{{'Написать e-mail'}}</v-button>
				<v-button flat regular @click="openContactsTab()">{{'Контакты'}}</v-button>
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

@Component
export default class DialogActiveOperationsEmail extends Vue {
	@Getter('page') public readonly page!: PageBase;
	@Prop({ type: Boolean, required: true }) readonly visible!: boolean;
	@Prop({ required: true }) readonly contact!: IAccContactInfo;

	mutableComments: string = this.contact.Comments!;

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

	private createEmail() {
		window.open('mailto:' + this.contact.ContactValue);
		//window.Sage.Link.editActivity('V6UJ9A00017N', false);
		this.$emit('close');
	}

	private openContactsTab() {
		this.page.showTab('FbContacts');
		this.$emit('close');
	}

	private async save() {
		if (this.mutableComments != this.contact.Comments) {
			const updatedContact = { ...this.contact };
			updatedContact.Comments = this.mutableComments;
			this.$emit('change', updatedContact);
		}
		this.$emit('close');

		await this.$store.dispatch(this.storeName + '/load', this.entityId); // перезагрузка страницы
	}

	private cancel() {
		this.$emit('close');
	}

	@Watch('comments')
	private updateComments() {
		if (this.contact.Comments) this.mutableComments = this.contact.Comments;
	}
}
</script>

<style lang="scss">
.v-dialog-ao-email {
	display: grid;
	grid-template-areas:
		'lblType txtType txtType txtType'
		'lblValue txtValue txtValue txtValue'
		'lblComment lblComment . .'
		'txtComment txtComment txtComment txtComment';
	grid-template-columns: 75px 156px 156px 156px;
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
</style>