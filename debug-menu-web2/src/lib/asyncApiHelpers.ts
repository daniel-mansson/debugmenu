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
        let category = categories.find(c => c.id == element.category);

        if (!category) {
            category = {
                id: element.category,
                groups: [],
                elements: []
            }
            categories.push(category)
        }

        let target = category;
        for (const [i, value] of element.groups.entries()) {
            if (i === element.groups.length - 1) {
                target.elements.push(element)
            }
            else {
                let next = target.groups.find(g => g.id == value);
                if (!next) {
                    next = {
                        id: value,
                        groups: [],
                        elements: []
                    };
                    target.groups.push(next);
                }
                target = next;
            }
        }
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
    console.log('hejj')
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

                console.log('hejj')
                console.log(operation.channels()[0])
                let category = '';
                if (channelParts.length > 1) {
                    // category = channelParts[0];
                    //  channelParts.shift();
                }

                commands.push({
                    id: channelParts[channelParts.length - 1],
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