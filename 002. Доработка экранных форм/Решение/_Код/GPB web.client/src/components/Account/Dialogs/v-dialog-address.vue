<template>
	<!--v-bind (сокращается как ':')-->
	<v-modal caption="Добавить/Изменить контактный адрес" :visible="visible" :width="500">
		<div class="dialog-address">
			<div class="dialog-address-type">
				<div class="dialog-address-type-lbl">
					<v-label :text="'Тип адреса:'" element="dlgAddress-pkl_Type" input />
				</div>
				<div class="dialog-address-type-pkl">
					<v-picklist id="dlgAddress-pkl_Type" bordered narrow noLabel picklistName="Тип адреса" v-model="address.Description" />
				</div>
			</div>
			<div class="dialog-address-primary">
				<div class="dialog-address-primary-lbl">
					<v-label :text="'Основной:'" element="dlgAddress-chk_Primary" />
				</div>
				<div class="dialog-address-primary-chk">
					<v-checkbox id="dlgAddress-chk_Primary" noLabel size="small" v-model="address.IsPrimary" />
				</div>
			</div>
			<div class="dialog-address-mailing">
				<div class="dialog-address-mailing-lbl">
					<v-label :text="'Почтовый:'" element="dlgAddress-chk_Mailing" />
				</div>
				<div class="dialog-address-mailing-check">
					<v-checkbox id="dlgAddress-chk_Mailing" noLabel size="small" v-model="address.IsMailing" />
				</div>
			</div>
			<div class="dialog-address-index">
				<div class="dialog-address-index-lbl">
					<v-label :text="'Индекс:'" element="dlgAddress-txt_Index" input />
				</div>
				<div class="dialog-address-index-txt">
					<v-text-input id="dlgAddress-txt_Index" bordered narrow noLabel v-model="address.PostalCode" />
				</div>
			</div>
			<div class="dialog-address-country">
				<div class="dialog-address-country-lbl">
					<v-label :text="'Страна:'" element="dlgAddress-txt_Country" input />
				</div>
				<div class="dialog-address-country-txt">
					<v-text-input id="dlgAddress-txt_Country" bordered narrow noLabel v-model="address.Country" />
				</div>
			</div>
			<div class="dialog-address-subject">
				<div class="dialog-address-subject-lbl">
					<v-label :text="'Субъект РФ:'" element="dlgAddress-txt_Subject" input />
				</div>
				<div class="dialog-address-subject-txt">
					<v-text-input id="dlgAddress-txt_Subject" bordered narrow noLabel v-model="address.State" />
				</div>
			</div>
			<div class="dialog-address-region">
				<div class="dialog-address-region-lbl">
					<v-label :text="'Регион:'" element="dlgAddress-txt_Region" input />
				</div>
				<div class="dialog-address-region-txt">
					<v-text-input id="dlgAddress-txt_Region" bordered narrow noLabel v-model="address.County" />
				</div>
			</div>
			<div class="dialog-address-city">
				<div class="dialog-address-city-lbl">
					<v-label :text="'Город:'" element="dlgAddress-txt_City" input />
				</div>
				<div class="dialog-address-city-txt">
					<v-text-input id="dlgAddress-txt_Region" bordered narrow noLabel v-model="address.City" />
				</div>
			</div>
			<div class="dialog-address-street">
				<div class="dialog-address-street-lbl">
					<v-label :text="'Улица/Квартал:'" element="dlgAddress-txt_Street" input />
				</div>
				<div class="dialog-address-street-txt">
					<v-text-input id="dlgAddress-txt_Street" bordered narrow noLabel v-model="address.Street" />
				</div>
			</div>
			<div class="dialog-address-house">
				<div class="dialog-address-house-lbl">
					<v-label :text="'Дом/Владение:'" element="dlgAddress-txt_House" input />
				</div>
				<div class="dialog-address-house-txt">
					<v-text-input id="dlgAddress-txt_House" bordered narrow noLabel v-model="address.House" />
				</div>
			</div>
			<div class="dialog-address-building">
				<div class="dialog-address-building-lbl">
					<v-label :text="'Корпус/Строение:'" element="dlgAddress-txt_Building" input />
				</div>
				<div class="dialog-address-building-txt">
					<v-text-input id="dlgAddress-txt_Building" bordered narrow noLabel v-model="address.Building" />
				</div>
			</div>
			<div class="dialog-address-apartment">
				<div class="dialog-address-apartment-lbl">
					<v-label :text="'Офис/Квартира:'" element="dlgAddress-txt_Apartment" input />
				</div>
				<div class="dialog-address-apartment-txt">
					<v-text-input id="dlgAddress-txt_Apartment" bordered narrow noLabel v-model="address.Office" />
				</div>
			</div>
			<div class="dialog-address-fulladdress">
				<div class="dialog-address-fulladdress-lbl">
					<v-label :text="'Адрес одной строкой:'" element="dlgAddress-txt_Fulladdress" />
				</div>
				<div class="dialog-address-fulladdress-txt">
					<v-textarea id="dlgAddress-txt_Fulladdress" noLabel :rows="3" v-model="address.FullAddress" />
				</div>
			</div>
			<div class="dialog-address-source">
				<div class="dialog-address-source-lbl">
					<v-label :text="'Источник:'" element="dlgAddress-txt_Source" input />
				</div>
				<div class="dialog-address-source-txt">
					<v-text-input id="dlgAddress-txt_Source" bordered narrow noLabel v-model="address.SourceName" readonly />
				</div>
			</div>
		</div>
		<template #footer>
			<div>
				<v-button flat regular @click="save()">{{'OK'}}</v-button>
				<v-button flat regular @click="cancel()">{{'Отмена'}}</v-button>
			</div>
		</template>
	</v-modal>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import PageBase from '@/base-components/abstract/v-page-base';
import Getter from '@/decorators/Getter';
import { IAddress, toClientViewFromSelect as addressToClientView } from '@/model/interfaces/IAddress';

@Component
export default class DialogAddressAddEdit extends Vue {
	@Getter('page') public readonly page!: PageBase;
	@Prop({ type: Boolean, required: true }) readonly visible!: boolean;
	@Prop({ required: true }) readonly address!: IAddress;

	save() {
		this.$emit('change', this.address);
		this.$emit('close');
	}

	cancel() {
		this.$emit('close');
	}
}
</script>

<style lang="scss">
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