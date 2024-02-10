import { PUBLIC_BACKEND_URL } from "$env/static/public";

type SvelteFetch = (input: RequestInfo | URL, init?: RequestInit | undefined) => Promise<Response>;

export type TeamDto = {
    name: string;
    id: number;
    type: string;
    icon: string;
};

export type ApplicationDto = {
    id: number;
    name: string;
};

export type RuntimeTokenDto = {
    id: number;
    name: string;
    description: string;
    token: string;
};

export type RunningInstanceDto = {
    id: number;
    deviceId: string | undefined;
    websocketUrl: string | undefined;
    connectedViewers: number;
    hasConnectedInstance: boolean;
    applicationId: number;
};

export function DebugMenuBackend(fetch: SvelteFetch, token: string) {
    return {
        getVersion: () => fetch(`${PUBLIC_BACKEND_URL}/version`, {
            method: 'GET',
            headers: {
                authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            }
        }),
        getTeamsByUser: (userId: string) => fetch(`${PUBLIC_BACKEND_URL}/api/teams/by-user/${userId}`, {
            method: 'GET',
            headers: {
                authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            }
        }),
        getApplicationsByTeam: (teamId: number) => fetch(`${PUBLIC_BACKEND_URL}/api/applications/by-team/${teamId}`, {
            method: 'GET',
            headers: {
                authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            }
        }),
        getApplication: (applicationId: number) => fetch(`${PUBLIC_BACKEND_URL}/api/applications/${applicationId}`, {
            method: 'GET',
            headers: {
                authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            }
        }),
        createApplication: (name: string, userId: string) => fetch(`${PUBLIC_BACKEND_URL}/api/applications`, {
            method: 'POST',
            headers: {
                authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                ownerUserId: userId,
                item: {
                    name: name
                }
            })
        }),
        createTeam: (name: string, userId: string) => fetch(`${PUBLIC_BACKEND_URL}/api/teams`, {
            method: 'POST',
            headers: {
                authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                ownerUserId: userId,
                item: {
                    name: name
                }
            })
        }),
        deleteApplication: (applicationId: number) => fetch(`${PUBLIC_BACKEND_URL}/api/applications/${applicationId}`, {
            method: 'DELETE',
            headers: {
                authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            }
        }),
        getTokensByApplication: (applicationId: number) => fetch(`${PUBLIC_BACKEND_URL}/api/applications/${applicationId}/tokens`, {
            method: 'GET',
            headers: {
                authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            }
        }),
        createToken: (applicationId: number, name: string, description: string) => fetch(`${PUBLIC_BACKEND_URL}/api/applications/${applicationId}/tokens`, {
            method: 'POST',
            headers: {
                authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                name: name,
                description: description
            })
        }),
        deleteToken: (tokenId: number) => fetch(`${PUBLIC_BACKEND_URL}/api/tokens/${tokenId}`, {
            method: 'DELETE',
            headers: {
                authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            }
        }),
        getInstancesByToken: (tokenId: number) => fetch(`${PUBLIC_BACKEND_URL}/api/tokens/${tokenId}/instances`, {
            method: 'GET',
            headers: {
                authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            }
        }),
    };
}