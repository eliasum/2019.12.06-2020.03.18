//import Address from '@/model/client/Address';
import Vue from 'vue';

import ComponentsOverviewComponent from './components-overview.vue';
if (process.env.NODE_ENV === 'development') {
	Vue.component('components-overview', ComponentsOverviewComponent);
}

import TabInDevelopment from './common/v-tab-development.vue';
Vue.component('v-tab-development', TabInDevelopment);

import PageAccountDetails from './Account/v-page-account-details.vue';
Vue.component('v-page-account-details', PageAccountDetails);

import TabContacts from './Account/Tabs/Contacts/v-tab-contacts.vue';
Vue.component('v-tab-contacts', TabContacts);

import TabFilials from './Account/Tabs/Filials/v-tab-filials.vue';
Vue.component('v-tab-filials', TabFilials);

import TabClientGroup from './Account/Tabs/ClientGroup/v-tab-clientGroup.vue';
Vue.component('v-tab-clientGroup', TabClientGroup);

import DialogFilialAddEdit from './Account/Dialogs/v-dialog-filial-addedit.vue';
Vue.component('v-dialog-filial-addedit', DialogFilialAddEdit);

import DialogClientGroupAddEdit from './Account/Dialogs/ClientGroup/v-dialog-clientGroup-addedit.vue';
Vue.component('v-dialog-clientGroup-addedit', DialogClientGroupAddEdit);

import DialogClientGroupConflictResolver from './Account/Dialogs/ClientGroup/v-dialog-clientGroup-conflictResolver.vue';
Vue.component('v-dialog-clientGroup-conflictResolver', DialogClientGroupConflictResolver);

import DialogClientGroupNewMainClient from './Account/Dialogs/ClientGroup/v-dialog-clientGroup-newMainClient.vue';
Vue.component('v-dialog-clientGroup-newMainClient', DialogClientGroupNewMainClient);

import DialogClientGroupAddNewClient from './Account/Dialogs/ClientGroup/v-dialog-clientGroup-addNewClient.vue';
Vue.component('v-dialog-clientGroup-addNewClient', DialogClientGroupAddNewClient);

import DialogActiveOperationsEmail from './Account/Dialogs/v-dialog-ao-email.vue';
Vue.component('v-dialog-ao-email', DialogActiveOperationsEmail);

import DialogContactAddEdit from './Account/Dialogs/v-dialog-contact-addedit.vue';
Vue.component('v-dialog-contact-addedit', DialogContactAddEdit);

import DialogActiveOperationsPhone from './Account/Dialogs/v-dialog-ao-phone.vue';
Vue.component('v-dialog-ao-phone', DialogActiveOperationsPhone);

import DialogIPQuestionary from './Account/Dialogs/InvestmentQuestionary/v-dialog-questionary-IP.vue';
Vue.component('v-dialog-questionary-IP', DialogIPQuestionary);

import TabABSClassifiers from './Account/Tabs/ABSСlassifiers/v-tab-abs-classifiers.vue';
Vue.component('v-tab-abs-classifiers', TabABSClassifiers);

import ModalABSClassifiers from './Account/Tabs/ABSСlassifiers/v-modal-abs-client.vue';
Vue.component('v-modal-abs-client', ModalABSClassifiers);

import ModalMark from './Account/Tabs/ABSСlassifiers/v-modal-mark.vue';
Vue.component('v-modal-mark', ModalMark);

import ModalABSManagement from './Account/Tabs/ABSСlassifiers/v-modal-abs-management.vue';
Vue.component('v-modal-abs-management', ModalABSManagement);

import TabQuestionaries from './Account/Tabs/Questionaries/v-tab-questionaries.vue';
Vue.component('v-tab-questionaries', TabQuestionaries);

import SubtabInvestmentQuestionaries from './Account/Tabs/Questionaries/v-subtab-questionaries-investment.vue';
Vue.component('v-subtab-questionaries-investment', SubtabInvestmentQuestionaries);

import DialogAddressAddEdit from './Account/Dialogs/v-dialog-address.vue';
Vue.component('v-dialog-address', DialogAddressAddEdit);

import OptionAlert from './Options/Alerts/v-tab-option-alert.vue';
Vue.component('v-tab-option-alert', OptionAlert);

/*import SubtabInvestmentQuestionaries from './Account/Tabs/Questionaries/v-subtab-questionaries-investment.vue';
Vue.component('v-tab-questionaries', SubtabInvestmentQuestionaries);*/

import QuestionaryTabIP from './Account/Dialogs/InvestmentQuestionary/v-questionary-tab-IP.vue';
Vue.component('v-questionary-tab-IP', QuestionaryTabIP);

import QuestionaryTabIPAssignment from './Account/Dialogs/InvestmentQuestionary/v-questionary-tab-IP-assignment.vue';
Vue.component('v-questionary-tab-IP-assignment', QuestionaryTabIPAssignment);
