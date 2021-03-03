import { DateTime } from 'luxon';
import { EntityState, EntityId } from '@/model/interfaces/EntityBase';
import { dateTimeFromSDataDate } from '@/utils/helpers';
/**
 * Интерфейс сущности AccountExt
 * SData: accountExts
 * displayName: AccountExt
 * displayNamePlural: null
 * stringExpression: null
 * tableName: ACCOUNT_EXT
 */
export interface IAccountExt {
	/**
	 * Клиентское свойство, отображает текущее состояние сущности
	 */
	$state: EntityState;

	/**
	 * ИД сущности
	 */
	$key?: EntityId | null;

	/**
	 * columnName: ACCOUNTID
	 * displayName: AccountId
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Стандартный ИД
	 * type.name:  StandardIdDataType
	 */
	AccountId?: EntityId | null;

	/**
	 * columnName: CREATEDATE
	 * displayName: CreateDate
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Дата и время
	 * type.name:  DateTimeDataType
	 */
	CreateDate?: DateTime | null;

	/**
	 * columnName: CREATEUSER
	 * displayName: CreateUser
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Стандартный ИД
	 * type.name:  StandardIdDataType
	 */
	CreateUser?: EntityId | null;

	/**
	 * columnName: MODIFYDATE
	 * displayName: ModifyDate
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Дата и время
	 * type.name:  DateTimeDataType
	 */
	ModifyDate?: DateTime | null;

	/**
	 * columnName: MODIFYUSER
	 * displayName: ModifyUser
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Стандартный ИД
	 * type.name:  StandardIdDataType
	 */
	ModifyUser?: EntityId | null;

	/**
	 * columnName: LAST_NAME
	 * displayName: LastName
	 * length:  128
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	LastName?: string | null;

	/**
	 * columnName: FIRST_NAME
	 * displayName: FirstName
	 * length:  128
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	FirstName?: string | null;

	/**
	 * columnName: MIDDLE_NAME
	 * displayName: MiddleName
	 * length:  128
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	MiddleName?: string | null;

	/**
	 * columnName: BIRTHDAY
	 * displayName: Birthday
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Дата и время
	 * type.name:  DateTimeDataType
	 */
	Birthday?: DateTime | null;

	/**
	 * columnName: MANAGER_NAME
	 * displayName: ManagerName
	 * length:  255
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	ManagerName?: string | null;

	/**
	 * columnName: OFFICE_NUM
	 * displayName: OfficeNum
	 * length:  3
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	OfficeNum?: string | null;

	/**
	 * columnName: OFFICE_NAME
	 * displayName: OfficeName
	 * length:  255
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	OfficeName?: string | null;

	/**
	 * columnName: OFFICE_ADDITIONAL
	 * displayName: OfficeAdditional
	 * length:  1024
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	OfficeAdditional?: string | null;

	/**
	 * columnName: LAST_NAME_ENG
	 * displayName: LastNameEng
	 * length:  128
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	LastNameEng?: string | null;

	/**
	 * columnName: FIRST_NAME_ENG
	 * displayName: FirstNameEng
	 * length:  128
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	FirstNameEng?: string | null;

	/**
	 * columnName: MIDDLE_NAME_ENG
	 * displayName: MiddleNameEng
	 * length:  128
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	MiddleNameEng?: string | null;

	/**
	 * columnName: IS_CITIZENSHIP_RESIDENT
	 * displayName: IsCitizenshipResident
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Истина/ложь
	 * type.name:  TrueFalseDataType
	 */
	IsCitizenshipResident?: boolean | null;

	/**
	 * columnName: CITIZENSHIP
	 * displayName: Citizenship
	 * length:  128
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	Citizenship?: string | null;

	/**
	 * columnName: PLACEMENT_VOLUME
	 * displayName: PlacementVolume
	 * length:  255
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	PlacementVolume?: string | null;

	/**
	 * columnName: IS_INNER_CLIENT
	 * displayName: IsInnerClient
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Истина/ложь
	 * type.name:  TrueFalseDataType
	 */
	IsInnerClient?: boolean | null;

	/**
	 * columnName: INNER_CLIENT_CRITERION
	 * displayName: InnerClientCriterion
	 * length:  4000
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	InnerClientCriterion?: string | null;

	/**
	 * columnName: SEX
	 * displayName: Sex
	 * length:  3
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	Sex?: string | null;

	/**
	 * columnName: REVENUE_SOURCE
	 * displayName: RevenueSource
	 * length:  4000
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	RevenueSource?: string | null;

	/**
	 * columnName: PROFESSION
	 * displayName: Profession
	 * length:  255
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	Profession?: string | null;

	/**
	 * columnName: WORK_PLACE
	 * displayName: WorkPlace
	 * length:  255
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	WorkPlace?: string | null;

	/**
	 * columnName: TITLE
	 * displayName: Title
	 * length:  255
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	Title?: string | null;

	/**
	 * columnName: RISK_TREND
	 * displayName: RiskTrend
	 * length:  1024
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	RiskTrend?: string | null;

	/**
	 * columnName: IS_TAX_RESIDENT
	 * displayName: Налоговое резидентство
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Истина/ложь
	 * type.name:  TrueFalseDataType
	 */
	IsTaxResident?: boolean | null;

	/**
	 * columnName: INN
	 * displayName: Inn
	 * length:  24
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	Inn?: string | null;

	/**
	 * columnName: PENSION_CARD_NUMBER
	 * displayName: PensionCardNumber
	 * length:  24
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	PensionCardNumber?: string | null;

	/**
	 * columnName: BIRTHPLACE
	 * displayName: Birthplace
	 * length:  255
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	Birthplace?: string | null;

	/**
	 * columnName: MARITAL_STATUS
	 * displayName: MaritalStatus
	 * length:  64
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	MaritalStatus?: string | null;

	/**
	 * columnName: SPOUSE_NAME
	 * displayName: SpouseName
	 * length:  255
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	SpouseName?: string | null;

	/**
	 * columnName: DEPENDENTS_COUNT
	 * displayName: DependentsCount
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Десятичное
	 * type.name:  DecimalDataType
	 */
	DependentsCount?: number | null;

	/**
	 * columnName: IS_FPO
	 * displayName: IsFpo
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Истина/ложь
	 * type.name:  TrueFalseDataType
	 */
	IsFpo?: boolean | null;

	/**
	 * columnName: IS_BANK_STATEMENTS
	 * displayName: IsBankStatements
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Истина/ложь
	 * type.name:  TrueFalseDataType
	 */
	IsBankStatements?: boolean | null;

	/**
	 * columnName: BANK_STATEMENTS_PERIOD
	 * displayName: BankStatementsPeriod
	 * length:  64
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	BankStatementsPeriod?: string | null;

	/**
	 * columnName: IS_HOME_BANK
	 * displayName: IsHomeBank
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Истина/ложь
	 * type.name:  TrueFalseDataType
	 */
	IsHomeBank?: boolean | null;

	/**
	 * columnName: SOURCE_MODIFYDATE
	 * displayName: SourceModifyDate
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Дата и время
	 * type.name:  DateTimeDataType
	 */
	SourceModifyDate?: DateTime | null;

	/**
	 * columnName: SESSIONID
	 * displayName: SessionId
	 * length:  24
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	SessionId?: string | null;

	/**
	 * columnName: SESSION_LOADDATE
	 * displayName: SessionLoadDate
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Дата и время
	 * type.name:  DateTimeDataType
	 */
	SessionLoadDate?: DateTime | null;

	/**
	 * columnName: SOURCE_DESCRIPTION
	 * displayName: SourceDescription
	 * length:  4000
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	SourceDescription?: string | null;

	/**
	 * columnName: FAMILY_RELATION_TYPE
	 * displayName: FamilyRelationType
	 * length:  128
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	FamilyRelationType?: string | null;

	/**
	 * columnName: FIRST_INPUTDATE
	 * displayName: FirstInputDate
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Дата и время
	 * type.name:  DateTimeDataType
	 */
	FirstInputDate?: DateTime | null;

	/**
	 * columnName: SOURCE_NAME
	 * displayName: SourceName
	 * length:  64
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	SourceName?: string | null;

	/**
	 * columnName: FULL_NAME_ENG
	 * displayName: FullNameEng
	 * length:  255
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	FullNameEng?: string | null;

	/**
	 * columnName: Age
	 * displayName: Age
	 * isNullable:  true
	 * isReadOnly:  true
	 * type.displayName:  Двойное
	 * type.name:  DoubleDataType
	 */
	Age?: number | null;

	/**
	 * columnName: AgeRange
	 * displayName: AgeRange
	 * length:  255
	 * isNullable:  true
	 * isReadOnly:  true
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	AgeRange?: string | null;

	/**
	 * columnName: LEADSOURCEID
	 * displayName: LeadsourceId
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Стандартный ИД
	 * type.name:  StandardIdDataType
	 */
	LeadsourceId?: EntityId | null;

	/**
	 * columnName: TYPE_CHANGE_REASON
	 * displayName: TypeChangeReason
	 * length:  4000
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	TypeChangeReason?: string | null;

	/**
	 * columnName: TYPE_CHANGE_COMMENT
	 * displayName: TypeChangeComment
	 * length:  4000
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	TypeChangeComment?: string | null;

	/**
	 * columnName: TYPE_CHANGE_DATE
	 * displayName: TypeChangeDate
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Дата и время
	 * type.name:  DateTimeDataType
	 */
	TypeChangeDate?: DateTime | null;

	/**
	 * columnName: IS_SPECIAL_VIP
	 * displayName: IsSpecialVIP
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Истина/ложь
	 * type.name:  TrueFalseDataType
	 */
	IsSpecialVIP?: boolean | null;

	/**
	 * columnName: RECOMMENDEDBY
	 * displayName: RecommendedBy
	 * length:  4000
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	RecommendedBy?: string | null;

	/**
	 * columnName: LAST_SOURCE_INT_DATE
	 * displayName: LastSourceIntDate
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Дата и время
	 * type.name:  DateTimeDataType
	 */
	LastSourceIntDate?: DateTime | null;

	/**
	 * columnName: ACCESS_NAME
	 * displayName: AccessName
	 * length:  1
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	AccessName?: string | null;

	/**
	 * columnName: LOGIN_DBO
	 * displayName: LoginDBO
	 * length:  64
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	LoginDBO?: string | null;

	/**
	 * columnName: MOBILE_SMS
	 * displayName: MobileSMS
	 * length:  11
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	MobileSMS?: string | null;

	/**
	 * columnName: IS_FAMILY_MEMBER
	 * displayName: IsFamilyMember
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Истина/ложь
	 * type.name:  TrueFalseDataType
	 */
	IsFamilyMember?: boolean | null;

	/**
	 * columnName: CLIENTGROUPCOUNT
	 * displayName: ClientGroupCount
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Целое число
	 * type.name:  IntegerDataType
	 */
	ClientGroupCount?: number | null;

	/**
	 * columnName: ISTHROUGHASSISTANTCOMM
	 * displayName: IsThroughAssistantComm
	 * length:  1
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	IsThroughAssistantComm?: string | null;

	/**
	 * columnName: FULL_ADDRESS_CORRESP
	 * displayName: FULL_ADDRESS_CORRESP
	 * length:  4000
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	FULLADDRESSCORRESP?: string | null;

	/**
	 * columnName: MAINCLIENTCARINFO
	 * displayName: MainClientCarInfo
	 * length:  128
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	MainClientCarInfo?: string | null;

	/**
	 * columnName: ISFATCA
	 * displayName: ISFATCA
	 * length:  1
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	ISFATCA?: string | null;

	/**
	 * columnName: ISCRS
	 * displayName: ISCRS
	 * length:  1
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	ISCRS?: string | null;

	/**
	 * columnName: DOCUMENT_TYPE
	 * displayName: DOCUMENT_TYPE
	 * length:  128
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	DOCUMENTTYPE?: string | null;

	/**
	 * columnName: DOCUMENT_SERIES
	 * displayName: DOCUMENT_SERIES
	 * length:  64
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	DOCUMENTSERIES?: string | null;

	/**
	 * columnName: ISCURRENCYRESIDENT
	 * displayName: IsCurrencyResident
	 * length:  1
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	IsCurrencyResident?: string | null;

	/**
	 * columnName: FORSPEAKLANGUAGE
	 * displayName: ForSpeakLanguage
	 * length:  32
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	ForSpeakLanguage?: string | null;

	/**
	 * columnName: FORWRITELANGUAGE
	 * displayName: ForWriteLanguage
	 * length:  32
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	ForWriteLanguage?: string | null;

	/**
	 * columnName: MAINFILIALINFO
	 * displayName: MainFilialInfo
	 * length:  512
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	MainFilialInfo?: string | null;

	/**
	 * columnName: MAINMANAGERASSISTENTID
	 * displayName: MainManagerAssistentID
	 * length:  12
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	MainManagerAssistentID?: string | null;

	/**
	 * columnName: INVESTCONSULTANTFIO
	 * displayName: InvestConsultantFIO
	 * length:  96
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	InvestConsultantFIO?: string | null;

	/**
	 * columnName: FIRST_INPUTDATE_PB
	 * displayName: FIRST_INPUTDATE_PB
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Дата и время
	 * type.name:  DateTimeDataType
	 */
	FIRSTINPUTDATEPB?: DateTime | null;

	/**
	 * columnName: CLIENTPOLICY
	 * displayName: ClientPolicy
	 * length:  64
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	ClientPolicy?: string | null;

	/**
	 * columnName: CLIENTPOLICYSEGMENT
	 * displayName: ClientPolicySegment
	 * length:  32
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	ClientPolicySegment?: string | null;

	/**
	 * columnName: AUMSUMCLIENTGROUP
	 * displayName: AUMSumClientGroup
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Целое число
	 * type.name:  IntegerDataType
	 */
	AUMSumClientGroup?: number | null;

	/**
	 * columnName: AUMSUMCLIENTGROUPSEGMENT
	 * displayName: AUMSumClientGroupSegment
	 * length:  32
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	AUMSumClientGroupSegment?: string | null;

	/**
	 * columnName: AUMSUMCLIENT
	 * displayName: AUMSumClient
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Целое число
	 * type.name:  IntegerDataType
	 */
	AUMSumClient?: number | null;

	/**
	 * columnName: AUMSUMCLIENTSEGMENT
	 * displayName: AUMSumClientSegment
	 * length:  32
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	AUMSumClientSegment?: string | null;

	/**
	 * columnName: CLIENTPOTENTIALSEGMENT
	 * displayName: ClientPotentialSegment
	 * length:  32
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	ClientPotentialSegment?: string | null;

	/**
	 * columnName: RISKPROFILENAME
	 * displayName: RiskProfileName
	 * length:  64
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	RiskProfileName?: string | null;

	/**
	 * columnName: CLIENTNOTES
	 * displayName: ClientNotes
	 * length:  4000
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	ClientNotes?: string | null;

	/**
	 * columnName: ACTIVEPACKAGENAME
	 * displayName: ActivePackageName
	 * length:  64
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	ActivePackageName?: string | null;

	/**
	 * columnName: Segment
	 * displayName: Segment
	 * length:  4000
	 * isNullable:  true
	 * isReadOnly:  true
	 * type.displayName:  Текст Unicode
	 * type.name:  UnicodeTextDataType
	 */
	Segment?: string | null;

	/**
	 * columnName: CLIENTNUMBER
	 * displayName: ClientNumber
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Целое число
	 * type.name:  IntegerDataType
	 */
	ClientNumber?: number | null;

	/**
	 * columnName: PRIVATE_BANKING_DATE
	 * displayName: PrivateBankingDate
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Дата и время
	 * type.name:  DateTimeDataType
	 */
	PrivateBankingDate?: DateTime | null;

	/**
	 * columnName: ISTAXRESIDENT
	 * displayName: Налоговое резидентство
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Истина/ложь
	 * type.name:  TrueFalseDataType
	 */
	IstaxresIdent?: boolean | null;

	/**
	 * columnName: RISK_LEVEL
	 * displayName: Склонность к риску
	 * length:  32
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	Risk_level?: string | null;

	/**
	 * columnName: LASTCOMPACTIVITYDATE
	 * displayName: LASTCOMPACTIVITYDATE
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Дата и время
	 * type.name:  DateTimeDataType
	 */
	LASTCOMPACTIVITYDATE?: DateTime | null;

	/**
	 * columnName: FIRSTPLANACTIVITYDATE
	 * displayName: FIRSTPLANACTIVITYDATE
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Дата и время
	 * type.name:  DateTimeDataType
	 */
	FIRSTPLANACTIVITYDATE?: DateTime | null;

	/**
	 * columnName: ISGPBANDAFFILIATE
	 * displayName: ISGPBANDAFFILIATE
	 * length:  1
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	ISGPBANDAFFILIATE?: string | null;

	/**
	 * columnName: CLIENTCOMMENTS
	 * displayName: ClientComments
	 * length:  4000
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	ClientComments?: string | null;

	/**
	 * columnName: CLIENTPHOTO
	 * displayName: CLIENTPHOTO
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Стандартный ИД
	 * type.name:  StandardIdDataType
	 */
	CLIENTPHOTO?: EntityId | null;

	/**
	 * columnName: ISPDL
	 * displayName: ISPDL
	 * length:  1
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Текст
	 * type.name:  TextDataType
	 */
	ISPDL?: string | null;

	/**
	 * columnName: OFFICIALPHONEID
	 * displayName: OfficialPhoneId
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Стандартный ИД
	 * type.name:  StandardIdDataType
	 */
	OfficialPhoneId?: EntityId | null;

	/**
	 * columnName: PRIVATEPHONEID
	 * displayName: PrivatePhoneId
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Стандартный ИД
	 * type.name:  StandardIdDataType
	 */
	PrivatePhoneId?: EntityId | null;

	/**
	 * columnName: EMAILID
	 * displayName: EmailId
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Стандартный ИД
	 * type.name:  StandardIdDataType
	 */
	EmailId?: EntityId | null;

	/**
	 * columnName: ADDRESSID
	 * displayName: AddressID
	 * isNullable:  true
	 * isReadOnly:  false
	 * type.displayName:  Стандартный ИД
	 * type.name:  StandardIdDataType
	 */
	AddressID?: EntityId | null;

	/**
	 * type.name:  EntityRelationship
	 */
	LeadSource?: { $key: EntityId } | null;

	/**
	 * type.name:  EntityRelationship
	 */
	Account?: { $key: EntityId } | null;
}

/**
 * Конвертирует то, что пришло из Sdata в клиентскую сущность.
 * @param {any} [entity] Сущность
 * @param {EntityState} [state] текущее клиентское состояние
 * @return {IAccountExt} Клиентская сущность
 */
export function toClientViewAccountExt(entity: any, state: EntityState): IAccountExt {
	return Object.assign(entity, {
		$state: state,
		CreateDate: <DateTime>dateTimeFromSDataDate(entity.CreateDate),
		ModifyDate: <DateTime>dateTimeFromSDataDate(entity.ModifyDate),
		Birthday: <DateTime>dateTimeFromSDataDate(entity.Birthday),
		SourceModifyDate: <DateTime>dateTimeFromSDataDate(entity.SourceModifyDate),
		SessionLoadDate: <DateTime>dateTimeFromSDataDate(entity.SessionLoadDate),
		FirstInputDate: <DateTime>dateTimeFromSDataDate(entity.FirstInputDate),
		TypeChangeDate: <DateTime>dateTimeFromSDataDate(entity.TypeChangeDate),
		LastSourceIntDate: <DateTime>dateTimeFromSDataDate(entity.LastSourceIntDate),
		FIRSTINPUTDATEPB: <DateTime>dateTimeFromSDataDate(entity.FIRSTINPUTDATEPB),
		PrivateBankingDate: <DateTime>dateTimeFromSDataDate(entity.PrivateBankingDate),
		LASTCOMPACTIVITYDATE: <DateTime>dateTimeFromSDataDate(entity.LASTCOMPACTIVITYDATE),
		FIRSTPLANACTIVITYDATE: <DateTime>dateTimeFromSDataDate(entity.FIRSTPLANACTIVITYDATE),
		IsTaxResident: entity.IsTaxResident === true || entity.IsTaxResident === 'T' ? true : false
	});
}

/**
 * Конвертирует клиентскую сущность в то, что отправится в Sdata.
 * Если типы полей не те, нужно сделать конверты(DateTime преобразуется в то, что надо само)
 */
export function toServerViewAccountExt(entity: IAccountExt) {
	if (!entity) return {};
	return Object.assign({}, entity, {});
}
