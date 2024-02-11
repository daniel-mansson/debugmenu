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

    return {
        buttons,
        logs,
        all
    }
}

function getByTagAndOp(document: AsyncAPIDocumentInterface, operationName: "publish" | "subscribe", tagName: string) {
    let buttons = [];
    for (let operation of document.allOperations()) {
        if (operation.id() === operationName) {
            let hasButtonTag = operation.tags().filterBy((t) => t.name() === tagName).length == 1;
            if (hasButtonTag) {
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

                let category = "/" + channelParts.slice(0, -1).join("/");

                buttons.push({
                    operation: operation,
                    channel: operation.channels()[0].id(),
                    channelParts: channelParts,
                    category: category,
                    fields: fields,
                    description: operation.description() ?? '',
                    tags: operation.tags().all()
                });
            }
        }
    }
    return buttons;
}