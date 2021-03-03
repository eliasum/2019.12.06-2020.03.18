<template>
	<div class="v-account-detail">
		<v-main-content-toolbar>
			<v-button
				v-if="CurrentUser && CurrentUser.hasAccess('ENTITIES/ACCOUNT/EDIT')"
				flat
				@click="entitySave"
				:title="$i18n.resource('global', 'save')"
			>{{$i18n.resource('global', 'save')}}</v-button>
			<v-button
				v-if="CurrentUser && CurrentUser.hasAccess('ENTITIES/ACCOUNT/DELETE')"
				flat
				@click="entityDelete"
				:title="$i18n.resource('global', 'delete')"
			>{{$i18n.resource('global', 'delete')}}</v-button>
		</v-main-content-toolbar>
		<div class="v-account-detail-grid" v-if="account && account.accountExtension">
			<div class="v-account-detail-leftGrid">
				<div class="v-account-detail-leftGrid-clientNumber">
					<v-label class="v-account-detail-leftGrid-clientNumber-label" :text="$r('clientNumber')" element="lnk_clientNumber" />
					<div class="v-account-detail-leftGrid-clientNumber-link">
						<v-link id="lnk_clientNumber" :text="clientNumber" :active="false" />
					</div>
				</div>
				<div class="v-account-detail-leftGrid-isHelperMain">
					<v-label class="v-account-detail-leftGrid-isHelperMain-label" :text="$r('helperMain')" element="chk_isHelperMain" />
					<v-checkbox
						id="chk_isHelperMain"
						class="v-account-detail-leftGrid-isHelperMain-checkbox"
						:value="isHelperMain"
						noLabel
						size="small"
						:readonly="true"
					/>
					<div class="v-account-detail-leftGrid-isHelperMain-link">
						<v-link :text="$r('helpersAndTheirContacts')" :red="isHelperMain" @click="openContactsTab()" />
					</div>
				</div>
				<div class="v-account-detail-leftGrid-top-data-1">
					<div class="v-account-detail-leftGrid-top-data-1-lblFullname">
						<v-label :text="$r('clientFullName')" element="lnk_clientFullName" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-1-lblBirthdate">
						<v-label :text="$r('birthdate')" element="lnk_birthdate" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-1-lblGender">
						<v-label :text="$r('gender')" element="lnk_gender" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-1-lblMaritalStatus">
						<v-label :text="$r('maritalStatus')" element="lnk_maritalStatus" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-1-lblClientGroup">
						<v-label :text="$r('clientGroup')" element="lnk_clientGroup" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-1-fullname container-overflow">
						<v-link
							v-tooltip.top="{ offsetX: 0, offsetY: -4, label: clientFullNameFull }"
							id="lnk_clientFullName"
							:text="clientFullName"
							@click="openDocumentsTab()"
						/>
					</div>
					<div class="v-account-detail-leftGrid-top-data-1-birthdate container-overflow">
						<v-link id="lnk_birthdate" :text="birthdate" @click="openDocumentsTab()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-1-gender container-overflow">
						<v-link id="lnk_gender" :text="gender" @click="openDocumentsTab()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-1-maritalStatus container-overflow">
						<v-link id="lnk_maritalStatus" :text="maritalStatus" @click="openClientGroupTab()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-1-clientGroup container-overflow">
						<v-link id="lnk_clientGroup" :text="clientGroupCount" @click="openClientGroupTab()" />
					</div>
				</div>
				<div class="v-account-detail-leftGrid-top-data-2">
					<div class="v-account-detail-leftGrid-top-data-2-lblOfficialPhone container-overflow">
						<v-label :text="$r('officialPhone')" element="lnk_officialPhone" />
						<p style="font-size: 9px;">
							(
							<dfn style="color: red">{{$r('officialPhoneCommentary')}}</dfn>)
						</p>
					</div>
					<div class="v-account-detail-leftGrid-top-data-2-lblPrivatePhone container-overflow">
						<v-label :text="$r('privatePhone')" element="lnk_privatePhone" />
						<p style="font-size: 9px;">
							(
							<dfn style="color: red">{{$r('privatePhoneCommentary')}}</dfn>)
						</p>
					</div>
					<div class="v-account-detail-leftGrid-top-data-2-lblE-mail">
						<v-label :text="$r('e-mail')" element="lnk_e-mail" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-2-lblAddress">
						<v-label :text="$r('address')" element="lnk_address" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-2-lblCar">
						<v-label :text="$r('car')" element="lnk_car" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-2-officialPhone container-overflow">
						<v-link id="lnk_officialPhone" :text="officialPhone" @click="lnkOfficialPhoneClick()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-2-privatePhone container-overflow">
						<v-link id="lnk_privatePhone" :text="privatePhone" @click="lnkPrivatePhoneClick()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-2-e-mail container-overflow">
						<v-link id="lnk_e-mail" v-tooltip.top="{ offsetX: 0, offsetY: -4, label: email }" :text="email" @click="lnkEmailClick()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-2-address container-overflow">
						<v-link id="lnk_address" v-tooltip.top="{ offsetX: 0, offsetY: -4, label: address }" :text="address" @click="lnkAddressClick()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-2-car container-overflow">
						<v-link id="lnk_car" v-tooltip.top="{ offsetX: 0, offsetY: -4, label: car }" :text="car" @click="openCarsTab()" />
					</div>
				</div>
				<div class="v-account-detail-leftGrid-top-data-3">
					<div class="v-account-detail-leftGrid-top-data-3-lbltitle">
						<v-label :text="$r('title')" element="lnk_title" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-3-lblprimaryEmployment">
						<v-label :text="$r('primaryEmployment')" element="lnk_primaryEmployment" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-3-lblGazprom">
						<v-label :text="$r('Gazprom')" element="lnk_Gazprom" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-3-lblPO">
						<v-label :text="$r('PO')" element="lnk_PO" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-3-lblFATCA">
						<v-label :text="$r('FATCA')" element="lnk_FATCA" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-3-lblCRS">
						<v-label :text="$r('CRS')" element="lnk_CRS" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-3-title container-overflow">
						<v-link id="lnk_title" :text="title" @click="openCorporateConnectionsTab()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-3-primaryEmployment container-overflow">
						<v-link
							id="lnk_primaryEmployment"
							v-tooltip.top="{ offsetX: 0, offsetY: -4, label: employment }"
							:text="employment"
							@click="openCorporateConnectionsTab()"
						/>
					</div>
					<div class="v-account-detail-leftGrid-top-data-3-Gazprom container-overflow">
						<v-link id="lnk_Gazprom" :text="Gazprom" @click="openCorporateConnectionsTab()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-3-PO container-overflow">
						<v-link id="lnk_PO" :text="PDL" @click="openCorporateConnectionsTab()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-3-FATCA container-overflow">
						<v-link id="lnk_FATCA" :text="FATCA" @click="openCorporateConnectionsTab()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-3-CRS container-overflow">
						<v-link id="lnk_CRS" :text="CRS" @click="openCorporateConnectionsTab()" />
					</div>
				</div>
				<div class="v-account-detail-leftGrid-top-data-4">
					<div class="v-account-detail-leftGrid-top-data-4-lbldocumentType">
						<v-label :text="$r('documentType')" element="lnk_documentType" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-4-lbldocumentNumber">
						<v-label :text="$r('documentNumber')" element="lnk_documentNumber" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-4-lblcitizenship">
						<v-label :text="$r('citizenship')" element="lnk_citizenship" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-4-lbltaxResidentRussia">
						<v-label :text="$r('taxResidentRussia')" element="lnk_taxResidentRussia" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-4-lblcurrencyResidentRussia">
						<v-label :text="$r('currencyResidentRussia')" element="lnk_currencyResidentRussia" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-4-lblspokenCommunicationLanguage">
						<v-label :text="$r('spokenCommunicationLanguage')" element="lnk_spokenCommunicationLanguage" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-4-lblwrittenCommunicationLanguage">
						<v-label :text="$r('writtenCommunicationLanguage')" element="lnk_writtenCommunicationLanguage" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-4-documentType container-overflow">
						<v-link id="lnk_documentType" :text="documentType " @click="openDocumentsTab()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-4-documentNumber container-overflow">
						<v-link
							id="lnk_documentNumber"
							v-tooltip.top="{ offsetX: 0, offsetY: -4, label: documentNumber }"
							:text="documentNumber"
							@click="openDocumentsTab()"
						/>
					</div>
					<div class="v-account-detail-leftGrid-top-data-4-citizenship container-overflow">
						<v-link
							id="lnk_citizenship"
							v-tooltip.top="{ offsetX: 0, offsetY: -4, label: citizenship }"
							:text="citizenship"
							@click="openDocumentsTab()"
						/>
					</div>
					<div class="v-account-detail-leftGrid-top-data-4-taxResidentRussia container-overflow">
						<v-link id="lnk_taxResidentRussia" :text="taxResidentRussia" @click="openDocumentsTab()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-4-currencyResidentRussia container-overflow">
						<v-link id="lnk_currencyResidentRussia" :text="currencyResidentRussia" @click="openDocumentsTab()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-4-spokenCommunicationLanguage container-overflow">
						<v-link id="lnk_spokenCommunicationLanguage" :text="spokenCommunicationLanguage" @click="openDocumentsTab()" />
					</div>
					<div class="v-account-detail-leftGrid-top-data-4-writtenCommunicationLanguage container-overflow">
						<v-link id="lnk_writtenCommunicationLanguage" :text="writtenCommunicationLanguage" @click="openDocumentsTab()" />
					</div>
				</div>
				<div class="v-account-detail-leftGrid-middle-data-1">
					<div class="v-account-detail-leftGrid-middle-data-1-lblprimaryBranch">
						<v-label :text="$r('primaryBranch')" element="lnk_primaryBranch" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-1-lblprimaryCM">
						<v-label :text="$r('primaryCM')" element="lnk_primaryCM" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-1-lblprimaryCMDeputy">
						<v-label :text="$r('primaryCMDeputy')" element="lnk_primaryCMDeputy" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-1-primaryBranch container-overflow">
						<v-link
							id="lnk_primaryBranch"
							v-tooltip.top="{ offsetX: 0, offsetY: -4, label: primaryBranch }"
							:text="primaryBranch"
							@click="openBranchesTab()"
						/>
					</div>
					<div class="v-account-detail-leftGrid-middle-data-1-primaryCM container-overflow">
						<v-link
							id="lnk_primaryCM"
							v-tooltip.top="{ offsetX: 0, offsetY: -4, label: primaryCM }"
							:text="primaryCM"
							@click="openBranchesTab()"
						/>
					</div>
					<div class="v-account-detail-leftGrid-middle-data-1-primaryCMDeputy container-overflow">
						<v-link
							id="lnk_primaryCMDeputy"
							v-tooltip.top="{ offsetX: 0, offsetY: -4, label: primaryCMDeputy }"
							:text="primaryCMDeputy"
							@click="openBranchesTab()"
						/>
					</div>
				</div>
				<div class="v-account-detail-leftGrid-middle-data-2">
					<div class="v-account-detail-leftGrid-middle-data-2-lblclientType">
						<v-label :text="$r('clientType')" element="lnk_clientType" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-2-lblservicePackage">
						<v-label :text="$r('servicePackage')" element="lnk_servicePackage" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-2-lblGazpromAttractionDate">
						<v-label :text="$r('GazpromAttractionDate')" element="lnk_GazpromAttractionDate" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-2-lblprivateBusinessAttractionDate">
						<v-label :text="$r('privateBusinessAttractionDate')" element="lnk_privateBusinessAttractionDate" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-2-lblprivateBusinessAttractionChannel">
						<v-label :text="$r('privateBusinessAttractionChannel')" element="lnk_privateBusinessAttractionChannel" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-2-clientType container-overflow">
						<v-link id="lnk_clientType" :text="clientType" @click="openSegmentationTab()" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-2-servicePackage container-overflow">
						<v-link id="lnk_servicePackage" :text="servicePackage" @click="lnkServicePackageClick()" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-2-GazpromAttractionDate container-overflow">
						<v-link id="lnk_GazpromAttractionDate" :text="GazpromAttractionDate" @click="openSegmentationTab()" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-2-privateBusinessAttractionDate container-overflow">
						<v-link id="lnk_privateBusinessAttractionDate" :text="privateBusinessAttractionDate" @click="openSegmentationTab()" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-2-privateBusinessAttractionChannel container-overflow">
						<v-link id="lnk_privateBusinessAttractionChannel" :text="privateBusinessAttractionChannel" @click="openSegmentationTab()" />
					</div>
				</div>
				<div class="v-account-detail-leftGrid-middle-data-3-labels">
					<div class="v-account-detail-leftGrid-middle-data-3-labels-lblclientPolicy">
						<v-label :text="$r('clientPolicy')" element="lnk_clientPolicy" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-3-labels-lblclientGroupAUM">
						<v-label :text="$r('clientGroupAUM')" element="lnk_clientGroupAUM" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-3-labels-lblclientAUM">
						<v-label :text="$r('clientAUM')" element="lnk_clientAUM" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-3-labels-lblPotential">
						<v-label :text="$r('Potential')" element="lnk_Potential" />
						<p style="font-size: 9px;">
							(
							<dfn style="color: red">{{$r('potentialCommentary')}}</dfn>)
						</p>
					</div>
				</div>
				<div class="v-account-detail-leftGrid-middle-data-3">
					<div class="v-account-detail-leftGrid-middle-data-3-clientPolicy container-overflow">
						<v-link id="lnk_clientPolicy" :text="clientPolicy" @click="openSegmentationTab()" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-3-clientGroupAUM container-overflow">
						<v-link id="lnk_clientGroupAUM" :text="clientGroupAUM" @click="openSegmentationTab()" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-3-clientAUM container-overflow">
						<v-link id="lnk_clientAUM" :text="clientAUM" @click="openSegmentationTab()" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-3-Potential container-overflow">
						<v-link id="lnk_Potential" :text="Potential" @click="openSegmentationTab()" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-3-clientPolicySegment container-overflow">
						<v-link
							v-tooltip.top="{ offsetX: 0, offsetY: -4, label: clientPolicySegment }"
							:text="clientPolicySegment"
							@click="openSegmentationTab()"
						/>
					</div>
					<div class="v-account-detail-leftGrid-middle-data-3-clientGroupAUMSegment container-overflow">
						<v-link :text="clientGroupAUMSegment" @click="openSegmentationTab()" />
					</div>
					<div class="v-account-detail-leftGrid-middle-data-3-clientAUMSegment container-overflow">
						<v-link :text="clientAUMSegment" @click="openSegmentationTab()" />
					</div>
				</div>
				<div class="v-account-detail-leftGrid-bottom-data-1-label">
					<v-label
						class="v-account-detail-leftGrid-bottom-data-1-lblinvestConsultant"
						:text="$r('investConsultant')"
						element="lnk_investConsultant"
					/>
				</div>
				<div class="v-account-detail-leftGrid-bottom-data-1 container-overflow">
					<v-link
						class="v-account-detail-leftGrid-bottom-data-1-investConsultant"
						id="lnk_investConsultant"
						:text="investConsultant"
						@click="openInvestmentTab()"
					/>
				</div>
				<div class="v-account-detail-leftGrid-bottom-data-2-labels">
					<v-label class="v-account-detail-leftGrid-bottom-data-2-labels-lblRiskProfile" :text="$r('RiskProfile')" element="lnk_RiskProfile" />
				</div>
				<div class="v-account-detail-leftGrid-bottom-data-2">
					<v-indicator
						:canvasWidth="256"
						:canvasHeight="29"
						:ratingValue="riskRatingValue"
						:ratingLevel="riskRatingLevel"
						:emptyValue="$r('emptyValueIndicatorMessage')"
					/>
				</div>
			</div>
			<div class="v-account-detail-rightGrid">
				<div class="v-account-detail-rightGrid-photo">
					<div class="v-account-detail-rightGrid-photo-img">
						<!--
                        v-bind:src="imageURL" - привязка адреса файла (URL), который будет загружаться, к источнику, 
                        v-if="imageURL && !attachmentManager.loading" - если есть загружаемая картинка и закончена 
                        загрузка картинки методом attachmentManager.loading
						-->
						<img :src="imageURL" v-if="imageURL && !attachmentManager.loading" />
					</div>
					<!--
                    С помощью атрибута ref для html-элемента устанавливается ключ, через который потом можно ссылаться на этот элемент, т.е. в данном случае к элементу input
                    можно получить доступ через свойство $refs.inputUploadFile типа интерфейса HTMLInputElement, который предоставляет специальные свойства и методы
                    для управления параметрами, макетом и представлением элементов <input>. 
                    v-show="false" - элемент <input> скрыт, т.к. добавление картинок реализовано через нажатие на кнопку <v-button>
					-->
					<input ref="inputUploadFile" type="file" v-show="false" @change="inputUploadFileFileChanged" accept="image/*" />
					<!--v-on:change или @change-->
					<v-button flat class="v-account-detail-rightGrid-photo-addButton" @click="btnDeletePhotoClick" v-if="imageURL">{{$r('deletePhoto')}}</v-button>
					<!--v-on:click или @click-->
					<v-button flat class="v-account-detail-rightGrid-photo-addButton" @click="btnUploadPhotoClick" v-else>{{$r('uploadPhoto')}}</v-button>
				</div>
				<div class="v-account-detail-rightGrid-commentLabel">
					<v-label :text="$r('Commentary')" element="txt_commentary" />
				</div>
				<div class="v-account-detail-rightGrid-comment">
					<v-textarea
						id="txt_commentary"
						:value="commentary"
						@change="commentaryUpdated"
						noLabel
						:placeholder="$r('commentaryPlaceholder')"
						:rows="2"
					/>
				</div>
				<div class="v-account-detail-rightGrid-lastCallLabel">
					<v-label :text="$r('LastCall')" element="lnk_lastCall" />
				</div>
				<div class="v-account-detail-rightGrid-lastCall">
					<v-link id="lnk_lastCall" :text="lastCall" @click="openActivitiesTab()" />
				</div>
				<div class="v-account-detail-rightGrid-plannedCallLabel">
					<v-label :text="$r('PlannedCall')" element="lnk_plannedCall" />
				</div>
				<div class="v-account-detail-rightGrid-plannedCall">
					<v-link id="lnk_plannedCall" :text="plannedCall" @click="openActivitiesTab()" />
				</div>
				<div class="v-account-detail-rightGrid-activitiesLabel">
					<v-label :text="$r('Activities')" element="lnk_activities" />
				</div>
				<div class="v-account-detail-rightGrid-activitiesCount">
					<v-link id="lnk_activities" red :text="activitiesCount" @click="openActivitiesTab()" />
				</div>
				<div class="v-account-detail-rightGrid-notificationsLabel">
					<v-label :text="$r('Notifications')" element="lnk_notifications" />
				</div>
				<div class="v-account-detail-rightGrid-notifications">
					<v-link id="lnk_notifications" red :text="notifications" @click="openNotificationsTab()" />
				</div>
				<div class="v-account-detail-rightGrid-notesLabel">
					<v-label :text="$r('Notes')" element="txt_notes" />
				</div>
				<div class="v-account-detail-rightGrid-notes">
					<v-textarea id="txt_notes" :value="notes" @change="notesUpdated" noLabel :placeholder="$r('notesPlaceholder')" :rows="3" />
				</div>
			</div>
		</div>
		<v-loader v-else />
		<v-dialog-ao-email
			v-if="dialogAOEmail"
			:visible="dlgActiveOperationsEmailVisible"
			:contact="dialogAOEmail"
			@close="dlgActiveOperationsEmailClosed($event)"
			@change="dlgActiveOperationsEmailCommentsChanged($event)"
		/>
		<v-dialog-ao-phone
			v-if="dialogAOPhone"
			:visible="dlgActiveOperationsPhoneVisible"
			:contact="dialogAOPhone"
			@close="dlgActiveOperationsPhoneClosed($event)"
			@change="dlgActiveOperationsPhoneChange($event)"
			:activities="activities"
		/>
		<v-dialog-contact-addedit
			v-if="dialogAEContact"
			:visible="dlgAddEditContactVisible"
			:contact="dialogAEContact"
			@change="contactDialogChange($event)"
			@close="contactDialogClosed()"
		/>
		<v-dialog-address
			v-if="dialogAEAddress"
			:visible="dlgAddEditAddressVisible"
			:address="dialogAEAddress"
			@change="addressDialogChange($event)"
			@close="addressDialogClosed()"
		/>
	</div>
</template>

<script lang="ts">
import PageBase from '@/base-components/abstract/v-page-base';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import Getter from '@/decorators/Getter';
import LockQueue from '@/decorators/LockQueue';
import Lock from '@/decorators/Lock';
import { AccountExtended } from '../../store/Account.store';
import formatLocal from '../../filters/formatLocal';
import responceWrappers from '../../utils/responceWrappers';
import attachmentManager from '@/utils/attachmentManager';
import { IAttachment } from '../../model/interfaces/IAttachment';
import { EntityId } from '../../model/interfaces/EntityBase';
import { IAccContactInfo } from '@/model/interfaces/IAccContactInfo';
import { IActivity, toClientViewFromSelect as activityToClientView } from '@/model/interfaces/IActivity';
import { escape } from 'sqlstring';
import { IAddress } from '../../model/interfaces/IAddress';

@Component
export default class PageAccountDetails extends PageBase {
	@Getter('Account/entity') private readonly account!: AccountExtended;

	private imageRequest!: Promise<Blob>;
	private imageURL: string | null = null; // закрытое строковое или null свойство класса PageAccountDetails

	private dialogAOEmail: IAccContactInfo | null = null;
	private dialogAOPhone: IAccContactInfo | null = null;
	private dialogAEContact: IAccContactInfo | null = null;
	private dialogAEAddress: IAddress | null = null;

	private activities: IActivity[] = [];

	private dlgActiveOperationsEmailVisible = false;
	private dlgActiveOperationsPhoneVisible = false;
	private dlgAddEditContactVisible = false;
	private dlgAddEditAddressVisible = false;

	$refs!: {
		inputUploadFile: HTMLInputElement;
	};

	private attachmentManager: attachmentManager = new attachmentManager(
		this.onUploadPhotoCompleted,
		this.onUploadPhotoError,
	);

	private get clientNumber(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ClientNumber) return '0';
		return this.account.accountExtension.ClientNumber.toString();
	}

	private get isHelperMain(): boolean {
		return this.account.accountExtension.IsThroughAssistantComm === 'T';
	}

	private isEmpty(value: string | null | undefined) {
		return !value || value.length === 0 || !value.trim();
	}

	private get clientFullName(): string {
		let result = '';
		if (!this.isEmpty(this.account.accountExtension.LastName)) result += this.account.accountExtension.LastName;
		if (!this.isEmpty(this.account.accountExtension.FirstName))
			result += ' ' + this.account.accountExtension.FirstName!.substring(0, 1) + '.';
		if (!this.isEmpty(this.account.accountExtension.MiddleName))
			result += ' ' + this.account.accountExtension.MiddleName!.substring(0, 1) + '.';
		return result;
	}

	private get clientFullNameFull(): string {
		let result = '';
		if (!this.isEmpty(this.account.accountExtension.LastName)) result += this.account.accountExtension.LastName;
		if (!this.isEmpty(this.account.accountExtension.FirstName)) result += ' ' + this.account.accountExtension.FirstName;
		if (!this.isEmpty(this.account.accountExtension.MiddleName))
			result += ' ' + this.account.accountExtension.MiddleName;
		return result;
	}

	private get birthdate(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.Birthday) return 'Добавить';
		return formatLocal(this.account.accountExtension.Birthday, 'dd.MM.yyyy');
	}

	private get gender(): string {
		if (!this.account || !this.account.accountExtension || this.isEmpty(this.account.accountExtension.Sex))
			return 'Добавить';
		return this.account.accountExtension.Sex!;
	}

	private get maritalStatus(): string {
		if (!this.account || !this.account.accountExtension || this.isEmpty(this.account.accountExtension.MaritalStatus))
			return 'Добавить';
		return this.account.accountExtension.MaritalStatus!;
	}

	private get clientGroupCount(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ClientGroupCount)
			return 'Добавить';
		return this.account.accountExtension.ClientGroupCount.toString();
	}

	private get officialPhone(): string {
		if (!this.account || !this.account.officialPhone || this.isEmpty(this.account.officialPhone.ContactValue))
			return 'Добавить';
		return this.account.officialPhone.ContactValue!;
	}

	private get privatePhone(): string {
		if (!this.account || !this.account.privatePhone || this.isEmpty(this.account.privatePhone.ContactValue))
			return 'Добавить';
		return this.account.privatePhone.ContactValue!;
	}

	private get email(): string {
		if (!this.account || !this.account.email || this.isEmpty(this.account.email.ContactValue)) return 'Добавить';
		return this.account.email.ContactValue!;
	}

	private get address(): string {
		// вычисляемое поле vue
		if (!this.account || !this.account.address || !this.account.address.FullAddress) return 'Добавить';
		return this.account.address.FullAddress;
	}

	private get car(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.MainClientCarInfo)
			return 'Добавить';
		return this.account.accountExtension.MainClientCarInfo;
	}

	private get title(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.Title) return 'Добавить';
		return this.account.accountExtension.Title;
	}

	private get employment(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.WorkPlace) return 'Добавить';
		return this.account.accountExtension.WorkPlace;
	}

	private get Gazprom(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ISGPBANDAFFILIATE)
			return 'Добавить';
		return this.account.accountExtension.ISGPBANDAFFILIATE === 'T' ? 'Да' : 'Нет';
	}

	private get PDL(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ISPDL) return 'Добавить';
		return this.account.accountExtension.ISPDL === 'T' ? 'Да' : 'Нет';
	}

	private get FATCA(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ISFATCA) return 'Добавить';
		return this.account.accountExtension.ISFATCA === 'T' ? 'Да' : 'Нет';
	}

	private get CRS(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ISCRS) return 'Добавить';
		return this.account.accountExtension.ISCRS === 'T' ? 'Да' : 'Нет';
	}

	private get documentType(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.DOCUMENTTYPE)
			return 'Добавить';
		return this.account.accountExtension.DOCUMENTTYPE;
	}

	private get documentNumber(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.DOCUMENTSERIES)
			return 'Добавить';
		return this.account.accountExtension.DOCUMENTSERIES;
	}

	private get citizenship(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.Citizenship)
			return 'Добавить';
		return this.account.accountExtension.Citizenship;
	}

	private get taxResidentRussia(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.IsTaxResident)
			return 'Добавить';
		return this.account.accountExtension.IsTaxResident ? 'Да' : 'Нет';
	}

	private get currencyResidentRussia(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.IsCurrencyResident)
			return 'Добавить';
		return this.account.accountExtension.IsCurrencyResident === 'T' ? 'Да' : 'Нет';
	}

	private get spokenCommunicationLanguage(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ForSpeakLanguage)
			return 'Добавить';
		return this.account.accountExtension.ForSpeakLanguage;
	}

	private get writtenCommunicationLanguage(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ForWriteLanguage)
			return 'Добавить';
		return this.account.accountExtension.ForWriteLanguage;
	}

	private get primaryBranch(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.MainFilialInfo)
			return 'Добавить';
		return this.account.accountExtension.MainFilialInfo;
	}

	private get primaryCM(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ManagerName)
			return 'Добавить';
		return this.account.accountExtension.ManagerName;
	}

	private get primaryCMDeputy(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.MainManagerAssistentID)
			return 'Добавить';
		return this.account.accountExtension.MainManagerAssistentID;
	}

	private get clientType(): string {
		if (!this.account || !this.account.account || !this.account.account.Type) return '';
		return this.account.account.Type;
	}

	private get servicePackage(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ActivePackageName)
			return 'Добавить';
		return this.account.accountExtension.ActivePackageName;
	}

	private get GazpromAttractionDate(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.FirstInputDate)
			return 'Добавить';
		return formatLocal(this.account.accountExtension.FirstInputDate, 'dd.MM.yyyy');
	}

	private get privateBusinessAttractionDate(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.FIRSTINPUTDATEPB)
			return 'Добавить';
		return formatLocal(this.account.accountExtension.FIRSTINPUTDATEPB, 'dd.MM.yyyy');
	}

	private get privateBusinessAttractionChannel(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.SourceName) return 'Добавить';
		return this.account.accountExtension.SourceName;
	}

	private get clientPolicy(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ClientPolicy) return '';
		return this.account.accountExtension.ClientPolicy;
	}

	private get clientPolicySegment(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ClientPolicySegment)
			return '';
		return this.account.accountExtension.ClientPolicySegment;
	}

	private get clientGroupAUM(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.AUMSumClientGroup) return '';
		return this.account.accountExtension.AUMSumClientGroup.toString();
	}

	private get clientGroupAUMSegment(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.AUMSumClientGroupSegment)
			return '0 млн.';
		return this.account.accountExtension.AUMSumClientGroupSegment;
	}

	private get clientAUM(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.AUMSumClient) return '0';
		return this.account.accountExtension.AUMSumClient.toString();
	}

	private get clientAUMSegment(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.AUMSumClientSegment)
			return '0 млн.';
		return this.account.accountExtension.AUMSumClientSegment;
	}

	private get Potential(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ClientPotentialSegment)
			return '0 млн.';
		return this.account.accountExtension.ClientPotentialSegment;
	}

	private get riskProfile(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.RiskProfileName) return '';
		return this.account.accountExtension.RiskProfileName.toString();
	}

	private get investConsultant(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.InvestConsultantFIO)
			return 'Добавить';
		return this.account.accountExtension.InvestConsultantFIO;
	}

	private get lastCall(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.LASTCOMPACTIVITYDATE)
			return '';
		return formatLocal(this.account.accountExtension.LASTCOMPACTIVITYDATE, 'dd.MM.yyyy');
	}

	private get plannedCall(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.FIRSTPLANACTIVITYDATE)
			return '';
		return formatLocal(this.account.accountExtension.FIRSTPLANACTIVITYDATE, 'dd.MM.yyyy');
	}

	private get activitiesCount(): string {
		return '12';
	}

	private get notifications(): string {
		return '13';
	}

	private get commentary(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ClientComments) return '';
		return this.account.accountExtension.ClientComments;
	}

	private get notes(): string {
		if (!this.account || !this.account.accountExtension || !this.account.accountExtension.ClientNotes) return '';
		return this.account.accountExtension.ClientNotes;
	}

	private get riskRatingValue(): number {
		return 2.2;
	}

	private get riskRatingLevel(): number {
		return 2;
	}

	private commentaryUpdated(value: string): void {
		this.$store.dispatch(this.storeName + '/commentaryUpdate', value);
	}

	private notesUpdated(value: string): void {
		this.$store.dispatch(this.storeName + '/notesUpdate', value);
	}

	protected async created() {
		await super.created();
		await this.$store.dispatch(this.storeName + '/load', this.entityId); // перезагрузка страницы

		// this.$alert(this.storeName);	!!!

		if (this.account.accountExtension.CLIENTPHOTO) {
			this.imageRequest = this.attachmentManager.downloadBlob(this.account.accountExtension.CLIENTPHOTO);
			this.imageURL = URL.createObjectURL(await this.imageRequest);
		}
	}

	@Lock
	public async openContactsTab(): Promise<void> {
		await this.$store.dispatch(this.storeName + '/showTab', 'FbContacts');
	}

	@Lock
	private async openDocumentsTab(): Promise<void> {
		await this.$store.dispatch(this.storeName + '/showTab', 'FbDocuments');
	}

	@Lock
	private async openClientGroupTab(): Promise<void> {
		await this.$store.dispatch(this.storeName + '/showTab', 'FbClientGroup');
	}

	private lnkOfficialPhoneClick(): void {
		if (!this.account.officialPhone || !this.account.officialPhone.$key)
			this.showDialog('v-dialog-contact-addedit', null);
		else this.showDialog('v-dialog-ao-phone', this.account.officialPhone);
	}

	private lnkPrivatePhoneClick(): void {
		if (!this.account.privatePhone || !this.account.privatePhone.$key)
			this.showDialog('v-dialog-contact-addedit', null);
		else this.showDialog('v-dialog-ao-phone', this.account.privatePhone);
	}

	private lnkEmailClick() {
		if (!this.account.email || !this.account.email.$key) this.showDialog('v-dialog-contact-addedit', null);
		else this.showDialog('v-dialog-ao-email', this.account.email);
	}

	// private lnkAddressClick(): void {
	//     this.openContactsTab();
	// }

	private lnkAddressClick(): void {
		if (!this.account.address || !this.account.address.$key) this.showDialog('v-dialog-address', null);
		else this.showDialog('v-dialog-address', this.account.address);
	}

	@Lock
	private async openCarsTab(): Promise<void> {
		await this.$store.dispatch(this.storeName + '/showTab', 'FbCars');
	}

	@Lock
	private async openCorporateConnectionsTab(): Promise<void> {
		await this.$store.dispatch(this.storeName + '/showTab', 'FbCorporateConnections');
	}

	@Lock
	private async openBranchesTab(): Promise<void> {
		await this.$store.dispatch(this.storeName + '/showTab', 'FbBranches');
	}

	@Lock
	private async openInvestmentTab(): Promise<void> {
		await this.$store.dispatch(this.storeName + '/showTab', 'FbInvestmentSupport');
	}

	@Lock
	private async openSegmentationTab(): Promise<void> {
		await this.$store.dispatch(this.storeName + '/showTab', 'FbSegmentation');
	}

	private lnkServicePackageClick(): void {
		this.openInDevelopmentTab();
	}

	@Lock
	private async openActivitiesTab(): Promise<void> {
		await this.$store.dispatch(this.storeName + '/showTab', 'ActivityList');
	}

	@Lock
	private async openNotificationsTab(): Promise<void> {
		await this.$store.dispatch(this.storeName + '/showTab', 'FbNotifications');
	}

	@Lock
	private async openInDevelopmentTab(): Promise<void> {
		await this.$store.dispatch(this.storeName + '/showTab', 'TabInDevelopment');
	}

	@Lock
	private async entitySave(): Promise<void> {
		if (!(await this.$store.dispatch(this.storeName + '/save', this.account.accountExtension)))
			this.$alert('Ошибка при сохранении сущности');
	}

	@Lock
	private async entityDelete(): Promise<void> {
		if (!(await this.$confirm(this.$r('deleteConfirm')))) return;
		if (!(await this.$store.dispatch(this.storeName + '/delete', this.account.account.$key)))
			this.$alert('Ошибка при удалении сущности');
		else window.open(`Account.aspx?modeid=list`, '_self');
	}

	@Lock
	private async btnUploadPhotoClick(): Promise<void> {
		if (this.account.accountExtension.CLIENTPHOTO) return; // проверка: если фото есть, отмена
		this.$refs.inputUploadFile.click(); // выполнение метода загрузки файла элементом <input> по событии @click, доступного по ссылке $refs.inputUploadFile
		await this.entitySave(); // сохранить сущность
	}

	@Lock
	private async btnDeletePhotoClick(): Promise<void> {
		// закрытый асинхронный метод удаления фото события нажатия на кнопку v-button
		if (!this.account.accountExtension.CLIENTPHOTO) return; // проверка: если фото отсутствует, отмена
		this.attachmentManager.deleteFile(this.account.accountExtension.CLIENTPHOTO); // выполнение метода удаления файла deleteFile на объекте класса attachmentManager
		this.account.accountExtension.CLIENTPHOTO = null; // фото отсутствует
		await this.entitySave(); // сохранить сущность
		this.imageURL = ''; // удалить путь к удаленному файлу
	}

	@Lock
	private inputUploadFileFileChanged(): void {
		if (this.$refs.inputUploadFile.files && this.$refs.inputUploadFile.files[0]) {
			this.attachmentManager.uploadFile(this.$refs.inputUploadFile.files[0]);
		}
	}

	@LockQueue(true)
	private async onUploadPhotoCompleted(key: EntityId) {
		if (this.account.accountExtension.CLIENTPHOTO)
			await this.attachmentManager.deleteFile(this.account.accountExtension.CLIENTPHOTO);
		this.account.accountExtension.CLIENTPHOTO = key;
		this.imageRequest = this.attachmentManager.downloadBlob(key);
		this.imageURL = URL.createObjectURL(await this.imageRequest);
		await this.entitySave();
		this.$refs.inputUploadFile.value = '';
	}

	@LockQueue(true)
	private onUploadPhotoError(): void {
		this.$alert(this.$r('uploadPhotoError'));
		this.$refs.inputUploadFile.files = null;
	}

	public async showDialog(dialogId: string, payload: object | null) {
		if (dialogId == 'v-dialog-ao-email') {
			this.showEmailDialog(payload as IAccContactInfo);
		} else if (dialogId == 'v-dialog-contact-addedit') {
			this.showContactDialog();
		} else if (dialogId == 'v-dialog-ao-phone') {
			await this.showPhoneDialog(payload as IAccContactInfo);
		} else if (dialogId == 'v-dialog-address') {
			this.showAddressDialog(payload as IAddress | null);
		} else super.showDialog(dialogId, payload);
	}

	private showAddressDialog(payload: IAddress | null) {
		if (!payload)
			payload = {
				EntityId: this.entityId,
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
		this.dialogAEAddress = payload;
		this.dlgAddEditAddressVisible = true;
	}

	private showEmailDialog(payload: IAccContactInfo) {
		this.dialogAOEmail = null;

		if (
			!payload.ContactType ||
			!payload.ContactValue ||
			(!payload.ContactType.toLowerCase().includes('e-mail') && !payload.ContactType.toLowerCase().includes('email'))
		)
			return;

		this.dialogAOEmail = payload;
		this.dlgActiveOperationsEmailVisible = true;
	}

	private showContactDialog() {
		this.dialogAEContact = {
			AccountId: this.entityId,
			ContactType: '',
			ContactValue: '',
			IsMain: false,
			$communications: [],
			Comments: '',
			SourceName: '',
			$state: 'new',
		};
		this.dlgAddEditContactVisible = true;
	}

	private async showPhoneDialog(payload: IAccContactInfo) {
		if (
			!payload.ContactType ||
			!payload.ContactValue ||
			!payload.ContactType.toLowerCase().includes('тел') ||
			!this.CurrentUser
		)
			return;

		const query = await this.$api.select(
			`select ACTIVITY.ACTIVITYID, ACTIVITY.STARTDATE, ACTIVITY.DESCRIPTION, USERINFO.USERNAME, ACTIVITY.LONGNOTES, ACTIVITY.STATUS from ACTIVITY inner join USERINFO on ACTIVITY.USERID = USERINFO.USERID where ACTIVITY.TYPE = '262146' and ACTIVITY.USERID = ${escape(
				this.CurrentUser.id,
			)}`,
			'$key, StartDate, Description, $UserName, LongNotes, Status',
		);

		this.activities = [];

		for (const object of query) {
			this.activities.push(activityToClientView(object, 'exist'));
		}

		this.dialogAOPhone = payload;
		this.dlgActiveOperationsPhoneVisible = true;
	}

	private dlgActiveOperationsEmailClosed() {
		this.dialogAOEmail = null;
		this.dlgActiveOperationsEmailVisible = false;
	}

	private dlgActiveOperationsEmailCommentsChanged(payload: IAccContactInfo) {
		if (!this.account || !payload) return;
		this.$store.dispatch(this.storeName + '/updateContact', payload);
	}

	@Lock
	private async contactDialogChange(contact: IAccContactInfo) {
		if (!contact) return;
		if (contact.AccountId) {
			await this.$store.dispatch(this.storeName + '/addContact', contact);
		}
	}

	@Lock
	private async addressDialogChange(address: IAddress) {
		if (!address) return;
		if (address.EntityId) await this.addUpdateAddress(address);
	}

	private async addUpdateAddress(address: IAddress) {
		console.log(address);
		if (address.$state == 'new') await this.$store.dispatch(this.storeName + '/addAddress', address);
		// add address
		else if (address.$state == 'exist') this.$store.dispatch(this.storeName + '/updateAddress', address); // update address
	}

	private contactDialogClosed() {
		this.dialogAEContact = null;
		this.dlgAddEditContactVisible = false;
	}

	private addressDialogClosed() {
		this.dialogAEAddress = null;
		this.dlgAddEditAddressVisible = false;
	}

	private dlgActiveOperationsPhoneClosed() {
		this.dialogAOPhone = null;
		this.dlgActiveOperationsPhoneVisible = false;
	}

	private dlgActiveOperationsPhoneChange(payload: IAccContactInfo) {
		if (!this.account || !payload) return;
		this.$store.dispatch(this.storeName + '/updateContact', payload);
	}
}
</script>

<style lang="scss">
#mainContentDetails {
	overflow-x: hidden;

	.mainContentContent {
		//margin: 0px;
		//background-color: rgb(240, 248, 248);

		.v-account-detail {
			//padding: 14px 10px 10px 10px;
			padding-top: 4px;
			overflow: auto;

			.container-overflow {
				text-overflow: ellipsis;
				white-space: nowrap;
				overflow: hidden;
			}

			&-grid {
				display: grid;
				grid-template-areas: 'leftGrid rightGrid';
				grid-template-columns: auto 205px;
				grid-column-gap: $distance-lg;
				min-width: 1240px;

				.v-account-detail-leftGrid {
					grid-area: leftGrid;
					display: grid;
					grid-template-areas:
						'clientNumber isHelperMain isHelperMain .'
						'top-data-1 top-data-2 top-data-3 top-data-4'
						'middle-data-1 middle-data-2 middle-data-3-labels middle-data-3'
						'bottom-data-1-label bottom-data-1 bottom-data-2-labels bottom-data-2';

					grid-template-columns: auto auto auto auto;
					grid-template-rows: auto auto auto auto;
					grid-column-gap: $distance-sm;
					grid-row-gap: $distance-md;

					&-clientNumber {
						grid-area: clientNumber;
						display: grid;
						grid-template-areas: 'label link';
						grid-template-columns: 115px minmax(100px, 200px);
						grid-gap: $distance-x2s;

						&-label {
							grid-area: label;
						}

						&-link {
							grid-area: link;
						}
					}
					&-isHelperMain {
						grid-area: isHelperMain;
						display: grid;
						grid-template-areas: 'label checkbox link';
						grid-template-columns: 230px 25px 165px;
						grid-column-gap: $distance-x2s;

						&-label {
							grid-area: label;
							color: red;
							font-weight: bold;
						}

						&-checkbox {
							grid-area: checkbox;
						}

						&-link {
							grid-area: link;
						}
					}

					&-top-data-1 {
						grid-area: top-data-1;
						display: grid;
						grid-template-areas:
							'lblFullName fullname'
							'lblBirthdate birthdate'
							'lblGender gender'
							'lblMaritalStatus maritalStatus'
							'lblClientGroup clientGroup';

						grid-template-columns: 115px minmax(100px, 200px);
						grid-gap: $distance-x2s;

						&-lblFullname {
							grid-area: lblFullName;
						}

						&-lblBirthdate {
							grid-area: lblBirthdate;
						}

						&-lblGender {
							grid-area: lblGender;
						}

						&-lblMaritalStatus {
							grid-area: lblMaritalStatus;
						}

						&-lblClientGroup {
							grid-area: lblClientGroup;
						}

						&-fullname {
							grid-area: fullname;
						}

						&-birthdate {
							grid-area: birthdate;
						}

						&-gender {
							grid-area: gender;
						}

						&-maritalStatus {
							grid-area: maritalStatus;
						}

						&-clientGroup {
							grid-area: clientGroup;
						}
					}

					&-top-data-2 {
						grid-area: top-data-2;
						display: grid;
						grid-template-areas:
							'lblOfficialPhone officialPhone'
							'lblPrivatePhone privatePhone'
							'lblE-mail e-mail'
							'lblAddress address'
							'lblCar car';

						grid-template-columns: 150px minmax(100px, 200px);
						grid-gap: $distance-x2s;

						&-lblOfficialPhone {
							grid-area: lblOfficialPhone;
						}

						&-lblPrivatePhone {
							grid-area: lblPrivatePhone;
						}

						&-lblE-mail {
							grid-area: lblE-mail;
						}

						&-lblAddress {
							grid-area: lblAddress;
						}

						&-lblCar {
							grid-area: lblCar;
						}

						&-officialPhone {
							grid-area: officialPhone;
						}

						&-privatePhone {
							grid-area: privatePhone;
						}

						&-e-mail {
							grid-area: e-mail;
						}

						&-address {
							grid-area: address;
						}

						&-car {
							grid-area: car;
						}
					}

					&-top-data-3 {
						grid-area: top-data-3;
						display: grid;
						grid-template-areas:
							'lbltitle title'
							'lblprimaryEmployment primaryEmployment'
							'lblGazprom Gazprom'
							'lblPO PO'
							'lblFATCA FATCA'
							'lblCRS CRS';

						grid-template-columns: 115px minmax(100px, 200px);
						grid-gap: $distance-x2s;

						&-lbltitle {
							grid-area: lbltitle;
						}

						&-lblprimaryEmployment {
							grid-area: lblprimaryEmployment;
						}

						&-lblGazprom {
							grid-area: lblGazprom;
						}

						&-lblPO {
							grid-area: lblPO;
						}

						&-lblFATCA {
							grid-area: lblFATCA;
						}

						&-lblCRS {
							grid-area: lblCRS;
						}

						&-title {
							grid-area: title;
						}

						&-primaryEmployment {
							grid-area: primaryEmployment;
						}

						&-Gazprom {
							grid-area: Gazprom;
						}

						&-PO {
							grid-area: PO;
						}

						&-FATCA {
							grid-area: FATCA;
						}

						&-CRS {
							grid-area: CRS;
						}
					}

					&-top-data-4 {
						grid-area: top-data-4;
						display: grid;
						grid-template-areas:
							'lbldocumentType documentType'
							'lbldocumentNumber documentNumber'
							'lblcitizenship citizenship'
							'lbltaxResidentRussia taxResidentRussia'
							'lblcurrencyResidentRussia currencyResidentRussia'
							'lblspokenCommunicationLanguage spokenCommunicationLanguage'
							'lblwrittenCommunicationLanguage writtenCommunicationLanguage';

						grid-template-columns: 150px minmax(100px, 200px);
						grid-gap: $distance-x2s;

						&-lbldocumentType {
							grid-area: lbldocumentType;
						}

						&-lbldocumentNumber {
							grid-area: lbldocumentNumber;
						}

						&-lblcitizenship {
							grid-area: lblcitizenship;
						}

						&-lbltaxResidentRussia {
							grid-area: lbltaxResidentRussia;
						}

						&-lblcurrencyResidentRussia {
							grid-area: lblcurrencyResidentRussia;
						}

						&-lblspokenCommunicationLanguage {
							grid-area: lblspokenCommunicationLanguage;
						}

						&-lblwrittenCommunicationLanguage {
							grid-area: lblwrittenCommunicationLanguage;
						}

						&-documentType {
							grid-area: documentType;
						}

						&-documentNumber {
							grid-area: documentNumber;
						}

						&-citizenship {
							grid-area: citizenship;
						}

						&-taxResidentRussia {
							grid-area: taxResidentRussia;
						}

						&-currencyResidentRussia {
							grid-area: currencyResidentRussia;
						}

						&-spokenCommunicationLanguage {
							grid-area: spokenCommunicationLanguage;
						}

						&-writtenCommunicationLanguage {
							grid-area: writtenCommunicationLanguage;
						}
					}

					&-middle-data-1 {
						grid-area: middle-data-1;
						display: grid;
						grid-template-areas:
							'lblprimaryBranch primaryBranch'
							'lblprimaryCM primaryCM'
							'lblprimaryCMDeputy primaryCMDeputy';

						grid-template-columns: 115px minmax(100px, 200px);
						grid-gap: $distance-x2s;

						&-lblprimaryBranch {
							grid-area: lblprimaryBranch;
						}

						&-lblprimaryCM {
							grid-area: lblprimaryCM;
						}

						&-lblprimaryCMDeputy {
							grid-area: lblprimaryCMDeputy;
						}

						&-primaryBranch {
							grid-area: primaryBranch;
						}

						&-primaryCM {
							grid-area: primaryCM;
						}

						&-primaryCMDeputy {
							grid-area: primaryCMDeputy;
						}
					}

					&-middle-data-2 {
						grid-area: middle-data-2;
						display: grid;
						grid-template-areas:
							'lblclientType clientType'
							'lblservicePackage servicePackage'
							'lblGazpromAttractionDate GazpromAttractionDate'
							'lblprivateBusinessAttractionDate privateBusinessAttractionDate'
							'lblprivateBusinessAttractionChannel privateBusinessAttractionChannel'
							'lblSource source';

						grid-template-columns: 150px minmax(100px, 200px);
						grid-gap: $distance-x2s;

						&-lblclientType {
							grid-area: lblclientType;
						}

						&-lblservicePackage {
							grid-area: lblservicePackage;
						}

						&-lblGazpromAttractionDate {
							grid-area: lblGazpromAttractionDate;
						}

						&-lblprivateBusinessAttractionDate {
							grid-area: lblprivateBusinessAttractionDate;
						}

						&-lblprivateBusinessAttractionChannel {
							grid-area: lblprivateBusinessAttractionChannel;
						}

						&-lblSource {
							grid-area: lblSource;
						}

						&-clientType {
							grid-area: clientType;
						}

						&-servicePackage {
							grid-area: servicePackage;
						}

						&-GazpromAttractionDate {
							grid-area: GazpromAttractionDate;
						}

						&-privateBusinessAttractionDate {
							grid-area: privateBusinessAttractionDate;
						}

						&-privateBusinessAttractionChannel {
							grid-area: privateBusinessAttractionChannel;
						}

						&-source {
							grid-area: source;
						}
					}

					&-middle-data-3-labels {
						grid-area: middle-data-3-labels;
						display: grid;
						grid-template-areas:
							'lblclientPolicy'
							'lblclientGroupAUM'
							'lblclientAUM'
							'lblPotential';

						grid-template-columns: 150px;
						grid-template-rows: auto auto auto auto;
						grid-row-gap: $distance-x2s;

						&-lblclientPolicy {
							grid-area: lblclientPolicy;
						}

						&-lblclientGroupAUM {
							grid-area: lblclientGroupAUM;
						}

						&-lblclientAUM {
							grid-area: lblclientAUM;
						}

						&-lblPotential {
							grid-area: lblPotential;
						}

						&-lblRiskProfile {
							grid-area: lblRiskProfile;
						}
					}
					&-middle-data-3 {
						grid-area: middle-data-3;
						display: grid;
						grid-template-areas:
							'clientPolicy clientPolicySegment'
							'clientGroupAUM clientGroupAUMSegment'
							'clientAUM clientAUMSegment'
							'. Potential';

						grid-template-columns: 150px minmax(100px, 200px);
						grid-gap: $distance-x2s;

						&-clientPolicy {
							grid-area: clientPolicy;
						}

						&-clientGroupAUM {
							grid-area: clientGroupAUM;
						}

						&-clientAUM {
							grid-area: clientAUM;
						}

						&-clientPolicySegment {
							grid-area: clientPolicySegment;
						}

						&-clientGroupAUMSegment {
							grid-area: clientGroupAUMSegment;
						}

						&-clientAUMSegment {
							grid-area: clientAUMSegment;
						}

						&-Potential {
							grid-area: Potential;
						}
					}

					&-bottom-data-1-label {
						grid-area: bottom-data-1-label;
					}

					&-bottom-data-1 {
						grid-area: bottom-data-1;
					}

					&-bottom-data-2-labels {
						grid-area: bottom-data-2-labels;
					}

					&-bottom-data-2 {
						grid-area: bottom-data-2;
						display: grid;
						grid-template-areas: 'RiskProfile clientRiskImage';

						grid-template-columns: 150px auto;
						grid-gap: $distance-x2s;

						&-RiskProfile {
							grid-area: RiskProfile;
						}

						&-clientRiskImage {
							grid-area: clientRiskImage;
							width: 86px;
							height: 26px;
							background-image: url(../../images/Gradusnik.png);
							background-size: cover;
							margin-top: -6px;
						}
					}
				}

				.v-account-detail-rightGrid {
					grid-area: rightGrid;
					display: grid;
					grid-template-areas:
						'photo photo'
						'commentLabel .'
						'comment comment'
						'lastCallLabel lastCall'
						'plannedCallLabel plannedCall'
						'activitiesLabel activitiesCount'
						'notificationsLabel notifications'
						'notesLabel .'
						'notes notes';

					grid-template-columns: 1fr 1fr;
					grid-template-rows: auto auto auto auto auto auto auto auto auto;
					grid-row-gap: $distance-xs;

					&-photo {
						grid-area: photo;
						text-align: center;

						&-img {
							margin: auto;
							width: 75px;
							height: 75px;

							img {
								width: 75px;
								height: 75px;
							}
						}
					}

					&-commentLabel {
						grid-area: commentLabel;
					}

					&-lastCallLabel {
						grid-area: lastCallLabel;
					}

					&-lastCall {
						grid-area: lastCall;
					}

					&-plannedCallLabel {
						grid-area: plannedCallLabel;
					}

					&-plannedCall {
						grid-area: plannedCall;
					}

					&-activitiesLabel {
						grid-area: activitiesLabel;
					}

					&-activitiesCount {
						grid-area: activitiesCount;
					}

					&-notificationsLabel {
						grid-area: notificationsLabel;
					}

					&-notifications {
						grid-area: notifications;
					}

					&-notesLabel {
						grid-area: notesLabel;
					}

					&-notes {
						grid-area: notes;
					}

					&-comment {
						grid-area: comment;
					}
				}
			}
		}
	}
}
</style>

<i18n>
{
	"default": {
        "clientNumber" : "Client number:",
        "helperMain" : "Primary communications through helper:",
        "helpersAndTheirContacts" : "Helpers and their contacts",
        "clientFullName" : "Client Fullname:",
        "birthdate" : "Birthdate:",
        "gender" : "Gender:",
        "maritalStatus" : "Marital status:",
        "clientGroup" : "Client group:",
        "officialPhone" : "Phone (official communications):",
        "privatePhone" : "Private phone (for Client Managers):",
        "e-mail": "E-mail:",
        "address": "Mail address:",
        "car": "Car number/model:",
        "title": "Title:",
        "primaryEmployment": "Primary employment:",
        "Gazprom": "Gazprom and affiliated companies:",
        "PO": "PO:",
        "FATCA": "FATCA:",
        "documentType": "Document type:",
        "documentNumber": "Series, number:",
        "citizenship": "Citizenship:",
        "taxResidentRussia": "Russian tax resident:",
        "currencyResidentRussia": "Russian currency resident:",
        "spokenCommunicationLanguage": "Language for spoken communication:",
        "writtenCommunicationLanguage": "Language for written communication:",
        "primaryBranch": "Primary branch:",
        "primaryCM": "Primary CM:",
        "primaryCMDeputy": "Primary CM Deputy:",
        "multyBranchClient": "Multibranch Client:",
        "investConsultant": "Invest consultant:",
        "clientType": "Client type:",
        "servicePackage": "Service package:",
        "GazpromAttractionDate": "Gazprom attraction date:",
        "privateBusinessAttractionDate": "PB attraction date:",
        "privateBusinessAttractionChannel": "PB attraction channel:",
        "clientPolicy": "Client policy:",
        "clientGroupAUM": "Client group AUM (RUB):",
        "clientAUM": "Client AUM (RUB):",
        "Potential": "Potential (assets in and out of GPB), RUB:",
        "RiskProfile": "Risk profile:",
        "Commentary": "Commentary:",
        "LastCall": "Last call/meeting:",
        "PlannedCall": "Planned call/meeting:",
        "Activities": "Activities:",
        "Notifications": "Notifications:",
        "Notes": "Notes:",
        "officialPhoneCommentary": "official GPB communication",
        "privatePhoneCommentary": "CM communication",
        "potentialCommentary": "assetes in and out of GPB",
        "CRS": "CRS:",
        "commentaryPlaceholder": "Input commentaries...",
        "notesPlaceholder": "Input notes...",
        "deleteConfirm": "Are you sure you want to delete this Client?",
        "uploadPhoto": "Upload photo",
        "deletePhoto": "Delete photo",
        "uploadPhotoError": "Error uploading photo",
        "emptyValueIndicatorMessage": "Value is not defined"
	},
	"ru-ru": {
        "clientNumber" : "Номер клиента:",
        "helperMain" : "Основное общение через помощника:",
        "helpersAndTheirContacts" : "Помощники и их контакты",
        "clientFullName" : "ФИО клиента:",
        "birthdate" : "Дата рождения:",
        "gender" : "Пол:",
        "maritalStatus" : "Семейное положение:",
        "clientGroup" : "Группа клиента:",
        "officialPhone" : "Тел. мобильный:",
        "privatePhone" : "Тел. мобильный Private:",
        "e-mail": "E-mail:",
        "address": "Адрес для корреспонденции:",
        "car": "Номер/Марка машины:",
        "title": "Должность:",
        "primaryEmployment": "Основное место работы:",
        "Gazprom": "Газпром и аффилированные компании:",
        "PO": "ПДЛ:",
        "FATCA": "FATCA:",
        "documentType": "Тип документа:",
        "documentNumber": "Серия, номер:",
        "citizenship": "Гражданство:",
        "taxResidentRussia": "Налоговый резидент РФ:",
        "currencyResidentRussia": "Валютный резидент РФ:",
        "spokenCommunicationLanguage": "Язык для устной коммуникации:",
        "writtenCommunicationLanguage": "Язык для письменной коммуникации:",
        "primaryBranch": "Основной филиал:",
        "primaryCM": "Основной КМ:",
        "primaryCMDeputy": "Зам. Основного КМ:",
        "multyBranchClient": "Мульти-филиальный клиент:",
        "investConsultant": "Инвестиционный консультант:",
        "clientType": "Тип клиента:",
        "servicePackage": "Пакет услуг:",
        "GazpromAttractionDate": "Дата привлечения в ГПБ:",
        "privateBusinessAttractionDate": "Дата привлечения в ЧБ:",
        "privateBusinessAttractionChannel": "Канал привлечения в ЧБ:",
        "clientPolicy": "Клиентская политика:",
        "clientGroupAUM": "АПУ группы клиента, руб:",
        "clientAUM": "АПУ клиента, руб:",
        "Potential": "Потенциал, руб:",
        "RiskProfile": "Риск-профиль:",
        "Commentary": "Комментарий:",
        "LastCall": "Последний звонок/встреча:",
        "PlannedCall": "Плановый звонок/встреча:",
        "Activities": "Активности:",
        "Notifications": "Уведомления:",
        "Notes": "Заметки:",
        "officialPhoneCommentary": "официальная коммуникация ГПБ",
        "privatePhoneCommentary": "для общения с КМ",
        "potentialCommentary": "активы в и вне ГПБ",
        "CRS": "CRS:",
        "commentaryPlaceholder": "Введите комментарии по клиенту...",
        "notesPlaceholder": "Введите заметки по клиенту...",
        "deleteConfirm": "Вы уверены, что хотите удалить этого клиента?",
        "uploadPhoto": "Загрузить фотографию",
        "deletePhoto": "Удалить фотографию",
        "uploadPhotoError": "Ошибка при загрузке фотографии",
        "emptyValueIndicatorMessage": "Значение не установлено"
	}
}
</i18n>