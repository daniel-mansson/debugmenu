import type { User } from "lucia";
import { get, writable } from "svelte/store";
import { DebugMenuBackend, type ApplicationDto, type TeamDto, type RuntimeTokenDto, type RunningInstanceDto } from "./backend/backend";
import { InstanceConnection } from "./InstanceConnection";

export const currentTeam = writable<number | undefined>(undefined);
export const currentApplication = writable<number | undefined>(undefined);
export const currentToken = writable<number | undefined>(undefined);
export const currentInstance = writable<string | undefined>(undefined);
export const currentUser = writable<User | undefined>(undefined);
export const currentBackendToken = writable<string | undefined>(undefined);

export const teams = writable<TeamDto[]>([]);
export const applications = writable<ApplicationDto[]>([]);
export const tokens = writable<RuntimeTokenDto[]>([]);
export const instances = writable<RunningInstanceDto[]>([]);

export const currentConnection = writable<InstanceConnection | undefined>(undefined);

type SvelteFetch = (input: RequestInfo | URL, init?: RequestInit | undefined) => Promise<Response>;
let pollInstancesInterval: any = undefined;

if (typeof window !== 'undefined') {
    currentToken.subscribe(token => {
        if (pollInstancesInterval) {
            clearInterval(pollInstancesInterval)
            pollInstancesInterval = undefined;
        }

        if (token) {
            pollInstancesInterval = setInterval(function () {
                fetchInstances(fetch)
            }, 15000);
        }
    })
}

if (typeof window !== 'undefined') {
    currentInstance.subscribe(instanceId => {
        let connection = get(currentConnection);
        let instance = get(instances).find(i => i.id == instanceId);

        //Disconnect if needed
        if (connection && connection.instance !== instance) {
            connection.stop();
            connection = undefined;
            currentConnection.set(undefined);
        }

        //Connect if needed
        if (!connection && instance?.websocketUrl) {
            //let token = get(tokens).find(t => t.id === get(currentToken))?.token;
            let token = get(currentBackendToken)
            console.log(token)
            if (token) {
                connection = new InstanceConnection(instance, token);
                currentConnection.set(connection);
            }
        }
    })
}

export async function updateUser(user: User | undefined, fetch: SvelteFetch) {
    if (typeof window !== 'undefined') {
        if (get(currentUser) === user) {
            return;
        }
        currentUser.set(user);
        await fetchTeams(fetch);
    }
}

export async function updateTeam(teamId: number | undefined, fetch: SvelteFetch) {
    if (typeof window !== 'undefined') {
        if (get(currentTeam) === teamId) {
            return;
        }

        currentApplication.set(undefined);
        currentToken.set(undefined);
        currentInstance.set(undefined);
        applications.set([]);
        tokens.set([]);
        instances.set([]);

        if (!get(teams).find(t => t.id === teamId)) {
            if (teamId) {
                currentTeam.set(undefined);
            }
            return;
        }
        currentTeam.set(teamId);
        await fetchApplications(fetch);
    }
}

export async function updateApplication(applicationId: number | undefined, fetch: SvelteFetch) {
    if (typeof window !== 'undefined') {
        if (get(currentApplication) === applicationId) {
            return;
        }

        currentToken.set(undefined);
        currentInstance.set(undefined);
        tokens.set([]);
        instances.set([]);

        if (!get(applications).find(t => t.id === applicationId)) {
            currentApplication.set(undefined);
            return;
        }
        currentApplication.set(applicationId);

        await fetchTokens(fetch);
    }
}

export async function updateToken(tokenId: number | undefined, fetch: SvelteFetch) {
    if (typeof window !== 'undefined') {
        if (get(currentToken) === tokenId) {
            return;
        }

        currentInstance.set(undefined);
        instances.set([]);

        if (!get(tokens).find(t => t.id === tokenId)) {
            currentToken.set(undefined);
            return;
        }
        currentToken.set(tokenId);

        await fetchInstances(fetch);
    }
}

export async function updateInstance(instanceId: string | undefined, fetch: SvelteFetch) {
    if (typeof window !== 'undefined') {
        if (get(currentInstance) === instanceId) {
            return;
        }
        if (!get(instances).find(t => t.id === instanceId)) {
            currentInstance.set(undefined);
            return;
        }
        currentInstance.set(instanceId);
    }
}

export async function fetchTeams(fetch: SvelteFetch) {
    if (typeof window !== 'undefined') {
        console.log('fetchTeams')
        let user = get(currentUser);
        if (user) {
            let response = await DebugMenuBackend(fetch, get(currentBackendToken)!).getTeamsByUser(user!.id);
            let data = await response.json();
            teams.set(data);
            if (!get(currentTeam) && data.length > 0) {
                await updateTeam(data[0].id, fetch)
            }
        }
        else {
            applications.set([])
            tokens.set([])
            instances.set([])
            teams.set([])
            currentTeam.set(undefined);
        }
    }
}

export async function fetchApplications(fetch: SvelteFetch) {
    if (typeof window !== 'undefined') {
        let teamId = get(currentTeam);
        console.log('fetchApplications t:' + teamId)
        if (teamId) {
            let response = await DebugMenuBackend(fetch, get(currentBackendToken)!).getApplicationsByTeam(teamId);
            let data = await response.json();
            console.log('fetchApplications r:' + JSON.stringify(data))
            applications.set(data);
        }
        else {
            applications.set([])
        }
    }
}

export async function fetchTokens(fetch: SvelteFetch) {
    if (typeof window !== 'undefined') {
        console.log('fetchTokens')
        let applicationId = get(currentApplication);
        if (applicationId) {
            let response = await DebugMenuBackend(fetch, get(currentBackendToken)!).getTokensByApplication(applicationId);
            let data = await response.json();
            tokens.set(data);
        }
        else {
            tokens.set([])
        }
    }
}

export async function fetchInstances(fetch: SvelteFetch) {
    if (typeof window !== 'undefined') {
        console.log('fetchInstances')
        let tokenId = get(currentToken);
        if (tokenId) {
            let response = await DebugMenuBackend(fetch, get(currentBackendToken)!).getInstancesByToken(tokenId);
            let data = await response.json();
            instances.set(data);
        }
        else {
            instances.set([])
        }
    }
}

export async function createApplication(fetch: SvelteFetch, name: string, teamId: number): Promise<ApplicationDto> {
    let response = await DebugMenuBackend(fetch, get(currentBackendToken)!).createApplication(name, teamId);
    let data = await response.json();
    applications.update(v => [...v, data])
    return data;
}

export async function createToken(fetch: SvelteFetch, applicationId: number, name: string, description: string): Promise<ApplicationDto> {
    let response = await DebugMenuBackend(fetch, get(currentBackendToken)!).createToken(applicationId, name, description);
    let data = await response.json();
    tokens.update(v => [...v, data])
    return data;
} 