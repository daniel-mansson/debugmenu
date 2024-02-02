import { writable } from "svelte/store";

export const currentTeam = writable<string | undefined>(undefined);
export const currentApplication = writable<string | undefined>(undefined);
export const currentToken = writable<string | undefined>(undefined);
export const currentInstance = writable<string | undefined>(undefined);

type NameIdPair = {
    name: string;
    id: string;
};

export const teams = writable<NameIdPair[]>([{ name: 'Personal', id: '1' }, { name: 'Team 1', id: '2' }, { name: 'Team 2', id: '3' }]);
export const applications = writable<NameIdPair[]>([{ name: 'Game Client 1', id: '1' }, { name: 'Game Client 2', id: '2' }, { name: 'Server', id: '3' }]);
export const tokens = writable<NameIdPair[]>([{ name: 'Dev', id: '1' }, { name: 'QA', id: '2' },]);
export const instances = writable<NameIdPair[]>([{ name: '34ccd0', id: '2' }, { name: '7fb609', id: '3' }, { name: 'fb029a', id: '1' }]);

