import type { User } from "lucia";
import { get, writable } from "svelte/store";
import { DebugMenuBackend, type TeamDto } from "./backend/backend";

export const currentTeam = writable<TeamDto | undefined>(undefined);
export const currentApplication = writable<string | undefined>(undefined);
export const currentToken = writable<string | undefined>(undefined);
export const currentInstance = writable<string | undefined>(undefined);
export const currentUser = writable<User | undefined>(undefined);
export const currentBackendToken = writable<string | undefined>(undefined);

type NameIdPair = {
    name: string;
    id: string;
};

export const teams = writable<TeamDto[]>([]);
export const applications = writable<NameIdPair[]>([{ name: 'Game Client 1', id: '1' }, { name: 'Game Client 2', id: '2' }, { name: 'Server', id: '3' }]);
export const tokens = writable<NameIdPair[]>([{ name: 'Dev', id: '1' }, { name: 'QA', id: '2' },]);
export const instances = writable<NameIdPair[]>([{ name: '34ccd0', id: '2' }, { name: '7fb609', id: '3' }, { name: 'fb029a', id: '1' }]);

if (typeof window !== 'undefined') {
    currentUser.subscribe(async (user) => {
        console.log('uuuser');
        if (user) {
            let response = await DebugMenuBackend(fetch, get(currentBackendToken)!).getTeamsByUser(user!.id);
            let data = await response.json();
            console.log(data);
            teams.set(data);
        }
        else {
            teams.set([])
        }
    });

}


