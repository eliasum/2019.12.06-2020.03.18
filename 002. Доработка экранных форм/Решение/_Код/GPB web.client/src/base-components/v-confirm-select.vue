<template>
	<div>
		<v-button-text-input ref="btnTextInput" class="v-tab-option-alert__button" :buttons="buttonsAlertProp" :value="resultValue" />
		<!--v-bind (сокращается как ':')-->

		<v-menu :visible="menuVisible" :left="menuLeft" :top="menuTop" :width="menuWidth" @hide="hide" class="v-tab-option-alert__menu">
			<div class="v-tab-option-alert__menu--main">
				<!--
                Для рендеринга коллекций предназначена директива v-for. Она имеет следующий синтаксис:
                v-for="item in items"
                Где items представляет массив, а item псевдоним для текущего перебираемого элемента из массива items.
                v-bind (сокращается как ':')
                Key - Для элементов в цикле принято добавлять атрибут key внутри списка v-for. 
                Vue использует атрибут key для создания уникальной привязки для каждой идентичности узла.
				-->
				<div v-for="alias in items" :key="alias.ItemId">
					<v-checkbox v-model="alias.selected" class="v-tab-option-alert__menu--newcheckbox">{{alias.Text}}</v-checkbox>
				</div>
			</div>

			<v-button @click="confirm">OK</v-button>
		</v-menu>
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
import { PickListItem } from './v-picklist.vue';

export interface PickList {
	DefaultCode: string | null;
	DefaultIndex: number | null;
	Filter: null;
	Language: string | null;
	PickListId: string;
	ShortText: string | null;
	Text: string;
	UserId: string;
	items: Array<PickListItem>; // массив объектов PickListItem
}
/*
export default - экспорт модуля только с классом ConfirmSelect для
доступа к компоненту v-confirm-select в других скриптах через любое имя
*/
@Component({ inheritAttrs: false })
export default class ConfirmSelect extends Vue {
	$refs!: {
		// ссылка на ButtonTextInputComponen, т.к. компонент в файле v-button-text-input.vue имеет класс ButtonTextInputComponent
		btnTextInput: ButtonTextInputComponent;
	};

	//-------------------------------------checkboxes, picklistName is props of ConfirmSelect---------------------------------------------------
	// строковое свойство, содержащее состояние чекбоксов, загружаемое из БД при загрузке страницы, только для чтения
	@Model('change', { type: String }) readonly checkboxes!: string | null; // Родитель <--> потомок @Model (двунаправленное сообщение)

	// строковое свойство, содержащее название справочника, которое считывается в родительском компоненте v-tab-option-alert
	@Prop({ type: String, required: true }) readonly picklistName!: string; // Родитель ---> потомок @Prop (управляет), только для чтения
	//------------------------------------------------------------------------------------------------------------------------------------------

	// разрешить кеширование
	readonly enableCache!: boolean;

	// ---------------items, loading, menuLeft, menuTop, menuVisible, menuWidth, mutableValue & picklist is data of ConfirmSelect---------------
	// свойство видимости меню
	menuVisible = false;

	// размеры меню
	menuWidth = '';
	menuLeft = 0;
	menuTop = 0;

	mutableValue: string | null = null; // строковое свойство состояния чекбоксов на клиенте, доступное для изменения
	picklist: PickList | null = null; // объект, реализующий интерфейс PickList
	loading = false; // загрузка в picklist

	/* 
	свойство items - массив <объектов PickListItem и объектов со 
	свойством selected типа boolean> - есть пустой массив. 
    Синтаксис создания пустого объекта в JavaScript:
    let user = new Object(); // синтаксис "конструктор объекта"
    let user = {};  // синтаксис "литерал объекта"
    */
	items: Array<PickListItem & { selected: boolean }> = [];

	//-------------------------------------buttonsAlertProp & resultValue is computed of ConfirmSelect------------------------------------------
	/*
	вычисляемое свойство для задания параметров кнопки компонента v-button-text-input, в т.ч. надписи на кнопке
	text: '...' и обработчика нажатия click: this.show. Возвращаемым значением является массив объектов 
	ButtonTextInputButton[], т.к. возвращаемое значение свойства buttonsAlertProp привязывается к атрибуту buttons
	тэга v-button-text-input с последующим реактивным обновлением значения атрибута buttons директивой v-bind,
	а возвращаемым значением свойства buttons компонента v-button-text-input является массив ButtonTextInputButton[],
	что видно из определения свойства @Prop(Array) readonly buttons!: ButtonTextInputButton[]; в файле v-button-text-input.vue
	*/
	get buttonsAlertProp(): ButtonTextInputButton[] {
		/*
		здесь возвращаемое значение массив из одного элемента, которым является объект со свойствами id, class, text, 
		transition, on, причем последнее свойство само является объектом со свойством click и значением this.show
		*/
		return [{ id: 'btn', class: 'class1', text: '...', transition: '', on: { click: this.show } }];
	}

	// вычисляемое свойство для вывода текста строки v-button-text-input
	get resultValue() {
		return this.items
			.filter(x => x.selected === true) // Возвращает элементы массива, которые удовлетворяют условию, указанному в функции обратного вызова.
			.map(x => x.Text) // Вызывает определенную функцию обратного вызова для каждого элемента массива и возвращает массив, содержащий результаты.
			.join(', '); // Добавляет все элементы массива, разделенные указанной строкой-разделителем.
	}
	//------------------------------------------------------------------------------------------------------------------------------------------

	// показать меню
	show() {
		/*
		в этом родительском компоненте ConfirmSelect (this) переходим к дочернему компоненту ButtonTextInputComponent по ссылке
		$refs.btnTextInput, далее переходим в компонент ButtonTextInputComponent как экземпляр DOM через $el и на нем вызываем 
		стандартный метод из DOM API C:\Users\user\.vscode\extensions\octref.vetur-0.24.0\server\node_modules\typescript\lib\lib.dom.d.ts,
		который получает клиентскую область, ограниченную прямоугольником с параметрами menuWidth, menuLeft и menuTop
		*/
		const rect = this.$refs.btnTextInput.$el.getBoundingClientRect();

		this.menuWidth = rect.width + 'px';
		this.menuLeft = rect.left;
		this.menuTop = rect.bottom;
		this.menuVisible = !this.menuVisible;
	}

	// обработка нажатия на кнопку ОК
	confirm() {
		// запись выводимого текста строки v-button-text-input в строковое свойство состояния чекбоксов на клиенте, доступное для изменения
		this.mutableValue = this.resultValue;

		/*
		Дочерний компонент v-confirm-select будет генерировать событие change с помощью вызова 
		this.$emit('change', this.mutableValue);, а родительский компонент v-tab-option-alert
		будет отлавливать это событие с помощью установки атрибута v-on:название_события, при
		получении события вызывать [метод, который принимает в качестве входных данных строковое 
		свойство состояния чекбоксов на клиенте this.mutableValue, доступное для изменения 
		и записывает его значение в БД]
		*/
		this.$emit('change', this.mutableValue);
		this.hide(); // скрыть меню
	}

	// скрыть меню
	hide() {
		this.setItems(); // установить значения из списка picklist в чекбоксы
		this.menuVisible = false; // скрыть меню
	}

	// установить значения из списка picklist в чекбоксы
	setItems() {
		console.log('В методе setItems()');

		// const свойство для массива объектов PickListItem[] списка или пустой массив
		const picklistItems = (this.picklist && this.picklist.items) || [];

		/*
		const свойство для массива строк string[], сформированных из текста строки v-button-text-input или пустой массив
		метод split() позволяет разбить строки на массив подстрок, используя заданную строку разделитель для определения места разбиения.
		*/
		const selectedItems = (this.mutableValue && this.mutableValue.split(', ')) || [];

		/*
		записать в массив <объектов PickListItem и объектов со свойством selected типа boolean>,
		которое является свойством этого компонента массив <объектов PickListItem и объектов со
		свойством selected типа boolean> t, сформированный из массива picklistItems методом map(),
		в котором через функцию обратного вызова создается объект t
		*/
		this.items = picklistItems.map(x => {
			/* 
            постоянная t типа объекта PickListItem & { selected: boolean } есть объект,
			созданный методом Object.assign, который копирует свойства объекта x (picklistItems) 
			в объект { selected: false }. То есть, свойства объекта x (picklistItems) копируются в первый
			объект { selected: false }. После копирования метод возвращает первый объединенный объект { selected: false, ... }.
            */
			const t: PickListItem & { selected: boolean } = Object.assign({ selected: false }, x);

			/*
			если есть массив строк, сформированных из текста строки v-button-text-input, и есть 
			свойство Text объекта t, взятого из объекта picklistItems, т.е. метка чекбокса и в массиве
			строк selectedItems содержится текст этой метки, то свойство t.selected = true, т.е.
			чекбокс выбран
			*/
			if (selectedItems && t.Text && selectedItems.includes(t.Text)) t.selected = true;
			return t;
		});
	}

	// загрузить справочник в picklist
	async loadPicklist(picklistName: string) {
		this.loading = true;

		const response = await this.$api.getPicklist(picklistName, this.enableCache);
		if (typeof response !== 'string' && 'PickListId' in response && 'items' in response) {
			this.picklist = response;
		} else {
			this.$toast({ body: `Список выбора '${picklistName}' не найден`, classes: 'failed', delay: -1 });
		}
		await this.$nextTick();
		this.loading = false;
	}

	// метод, выполняемый при создании компонента v-confirm-select:
	async created() {
		/*
		запись состояния чекбоксов checkboxes из БД (только для чтения) в строковое свойство 
		состояния чекбоксов на клиенте, доступное для изменения
		*/
		this.mutableValue = this.checkboxes;

		// подождать полную загрузку справочника (await), и только после этого ...
		await this.loadPicklist(this.picklistName);

		// ... установить значения из списка picklist в чекбоксы
		this.setItems();
	}

	/*
	@Watch('checkboxes') запускает наблюдение за свойством checkboxes на предмет изменений. 
	Изменения происходят:
	1. при создании компонента v-confirm-select(загрузка/перезагрузка страницы с компонентом)
	2. при записи в БД новых значений чекбоксов
	*/
	@Watch('checkboxes')
	onCheckboxes() {
		this.$alert('!!!');
		/*
		если входящее значение строкового свойства, содержащее состояние чекбоксов, загружаемое из БД при загрузке
		страницы такое же, как и соответствующее строковое свойство состояния чекбоксов на клиенте, доступное для
		изменения, то возврат из метода
		*/
		if (this.mutableValue == this.checkboxes) return;

		// загрузить состояние чекбоксов из БД
		this.mutableValue = this.checkboxes;

		// установить значения из списка picklist в чекбоксы
		this.setItems();
	}
}
</script> 

<style lang="scss">
// общий класс стилей
.v-tab-option-alert {
	$self: &; // ссылка на родителя
}

// стиль компонента v-button-text-input
.v-tab-option-alert__button {
	$self: &; // ссылка на родителя
	width: 25%;
}

// стиль компонента v-menu
.v-tab-option-alert__menu {
	$self: &; // ссылка на родителя

	//  конкатенация (соединение) посредством символа — & (класс v-tab-option-alert__menu--main)
	&--main {
		@include scroll($faint-weak-color, $accent-color, 4px); // включение миксина прокрутки

		max-height: 120px;

		/* Мы создаем grid контейнер, объявляя display: grid или display: inline-grid на элементе. 
		Как только мы это сделаем, все прямые дети этого элемента станут элементами сетки. */
		display: grid;

		row-gap: $distance-xs; // расстояние (промежуток) между строками в макете сетки

		/* Свойство overflow-y управляет отображением содержания блочного элемента по вертикали, 
		если контент целиком не помещается и выходит за область сверху или снизу от блока.
		auto - Вертикальная полоса прокрутки добавляется только при необходимости. */
		overflow-y: auto;
	}

	//  конкатенация (соединение) посредством символа — & (класс v-tab-option-alert__menu--newcheckbox)
	&--newcheckbox {
		.v-input-control > .v-input-control-inner > .v-input-control-inner i.v-icon:before {
			background-position: 1px 0;
			background-repeat: no-repeat;
			content: ' ';
			display: inline-block;
			height: 21px;
			vertical-align: middle;
			width: 24px;
			background-image: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABIAAAASCAAAAABzpdGLAAAAAnRSTlMAAHaTzTgAAAAmSURBVHgBYyAazEQCCBEEgAq9/Q8Fb+FC/+FgUAjhcyp+TxIdVgAaz5pRclAmbwAAAABJRU5ErkJggg==');
			border: unset;
			border-radius: unset;
			font-size: unset;
			padding: unset;
			min-width: unset;
			min-height: unset;
			font-weight: unset;
			z-index: unset;
		}
		// .class1 > .class2 > class3 - class3 дочерний для class2, который дочерний для class1
		.v-input-control > .v-input-control-inner > .v-input-control-inner i.v-icon.v-checkbox--checked:before {
			background-image: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABIAAAASCAMAAABhEH5lAAAACVBMVEUAAAATo/f///8qqXS/AAAAAXRSTlMAQObYZgAAADVJREFUeNqNzjEOACAIADHh/482cShoHLix061x0SKtlxIRRFAXlKQoCTrdE6QoCNKfGJm2AZVnAN96+MmKAAAAAElFTkSuQmCC');
			background-size: unset;
			background-position: unset;
			background-repeat: no-repeat;
		}
	}
}
</style>