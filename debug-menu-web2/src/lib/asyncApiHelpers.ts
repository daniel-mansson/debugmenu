import Parser, { type AsyncAPIDocumentInterface } from "@asyncapi/parser";

export async function parseAsyncApi(asyncApiYaml: string) {
    const parser = new Parser();
    let result = await parser.parse(asyncApiYaml);

    if (!result.document) {
        return {
            error: result.diagnostics
        }
    }

    let document = result.document;

    let buttons = getByTagAndOp(document, "publish", "button");
    let logs = getByTagAndOp(document, "subscribe", "log");

    let all = buttons.concat(logs);

    let categories: any = [];

    for (let element of all) {
        let category = categories[element.category] ?? {};

        // category.id = element.category;
        // category.groups ??= {};
        // category.elements ??= [];

        // a/b/c
        let target = category;
        for (const [i, value] of element.groups.entries()) {
            if (i === element.groups.length - 1) {
                target[value] = element;
            }
            else if (!target[value]) {
                target[value] = {}
            }
            target = target[value]
        }

        categories[element.category] = category;
    };

    return {
        buttons,
        logs,
        all,
        categories
    }
}

function getByTagAndOp(document: AsyncAPIDocumentInterface, operationName: "publish" | "subscribe", tagName: string) {
    let commands = [];
    for (let operation of document.allOperations()) {
        if (operation.id() === operationName) {
            let hasOpTag = operation.tags().filterBy((t) => t.name() === tagName).length == 1;
            if (hasOpTag) {
                let fields = [];
                for (let message of operation.messages()) {
                    let properties = message.payload()!.properties()!;
                    for (let propName of Object.keys(properties)) {
                        let prop = properties[propName];
                        fields.push({
                            type: prop.type(),
                            name: propName,
                            description: prop.description(),
                            format: prop.format()
                        });
                    }
                }

                let channelParts = operation.channels()[0].id().split('/');

                let category = '';
                if (channelParts.length > 1) {
                    // category = channelParts[0];
                    //  channelParts.shift();
                }

                commands.push({
                    __id: operation.channels()[0].id(),
                    type: tagName,
                    operation: operation,
                    channel: operation.channels()[0].id(),
                    groups: channelParts,
                    category: category,
                    fields: fields,
                    description: operation.description() ?? '',
                    tags: operation.tags().all()
                });
            }
        }
    }
    return commands;
}