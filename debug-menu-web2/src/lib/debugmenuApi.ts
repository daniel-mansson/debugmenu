import { writable } from "svelte/store";

export function parseDebugmenuApi(apiJson: string) {
    let api = JSON.parse(apiJson);
    //TODO: Validate
    return api;
}

function buildChannel([key, value]: [string, any]) {
    let groups = key.split('/');
    let fallbackName = groups[groups.length - 1];
    groups.pop();

    console.log(key)

    console.log(value)
    return {
        channel: key,
        name: value.name ?? fallbackName,
        groups: groups,
        category: value.category,
        type: value.type,
        settings: value.settings ?? {},
        publish: value.publish,
        subscribe: value.subscribe,
        state: writable<any>()
    };
}

export function buildDebugmenuElements(apiJson: string) {
    let api = parseDebugmenuApi(apiJson);

    let all = Object.entries(api.channels).map(buildChannel);

    let categories: any = [];

    for (let channel of all) {
        let category = categories.find(c => c.id == channel.category);

        if (!category) {
            category = {
                id: channel.category,
                groups: [],
                channels: []
            }
            categories.push(category)
        }

        let target = category;
        for (const [i, value] of channel.groups.entries()) {
            let next = target.groups.find(g => g.id == value);
            if (!next) {
                next = {
                    id: value,
                    groups: [],
                    channels: []
                };
                target.groups.push(next);
            }
            target = next;
            if (i === channel.groups.length - 1) {
                target.channels.push(channel)
            }
        }
    };

    console.log(categories)

    return {
        channels: all,
        categories: categories
    }
}
