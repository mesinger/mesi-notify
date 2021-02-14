import {LitElement, html, customElement, property, internalProperty, PropertyValues} from "lit-element";
import axios from "axios";

@customElement('execute-command-form')
export class ExecuteCommandFormComponent extends LitElement {
    constructor() {
        super();
    }

    public createRenderRoot(): LitElement {
        return this;
    }

    protected update(changedProperties: PropertyValues) {
        this.requiredProperties = JSON.parse(this.required);
        this.optionalProperties = JSON.parse(this.optional);
        super.update(changedProperties);
    }

    @property()
    commandName!: string;

    @property()
    required!: string;

    @property()
    optional!: string;

    @internalProperty()
    requiredProperties!: CommandProperty[];

    @internalProperty()
    optionalProperties!: CommandProperty[];

    public render() {
        return html`
            <div>
                ${this.requiredProperties.map(prop => html`
                        <div class="form-group">
                            <label for="${this.getInputFieldIdForProperty(prop)}">${prop.name}</label>
                            <input type="text" class="form-control" id="${this.getInputFieldIdForProperty(prop)}" .value="${prop.value}" placeholder="..." required>
                        </div>
                    `)}
                ${this.optionalProperties.map(prop => html`
                        <div class="form-group">
                            <label for="${this.getInputFieldIdForProperty(prop)}">${prop.name}</label>
                            <input type="text" class="form-control" id="${this.getInputFieldIdForProperty(prop)}" .value="${prop.value}" placeholder="...">
                        </div>
                    `)}
                <button type="submit" class="btn btn-primary" @click="${this.onSubmit}">Submit</button>
            </div>
        `;
    }

    private onSubmit() {
        this.requiredProperties = this.requiredProperties.map(prop => {
            const inputField = <HTMLInputElement>document.getElementById(this.getInputFieldIdForProperty(prop));
            const newValue = inputField.value;
            return {name: prop.name, value: newValue}
        });

        this.optionalProperties = this.optionalProperties.map(prop => {
            const inputField = <HTMLInputElement>document.getElementById(this.getInputFieldIdForProperty(prop));
            const newValue = inputField.value;
            return {name: prop.name, value: newValue}
        });

        const properties = this.requiredProperties.concat(this.optionalProperties);

        const propertiesData: {[id: string]: string; } = {};
        properties.forEach(property => propertiesData[property.name] = property.value);

        axios.post(`/api/command/execute?commandName=${this.commandName}&properties=${JSON.stringify(propertiesData)}`).then(rsp => {

        });
    }

    private getInputFieldIdForProperty(property: CommandProperty): string {
        return `${property.name}Input`;
    }
}

interface CommandProperty {
    name: string;
    value: string;
}
