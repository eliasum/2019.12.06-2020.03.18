export default interface ButtonTemplate {
	id: string | number;
	class: string | null;
	text: string | null;
	transition?: string | null;
	on?: object;
	attributes?: object;
	tooltip?: string;
}
