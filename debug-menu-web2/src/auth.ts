
import { SvelteKitAuth } from "@auth/sveltekit"
import GitHub from "@auth/sveltekit/providers/github"
import { GOOGLE_ID, GOOGLE_SECRET } from "$env/static/private"

export const { handle, signIn, signOut } = SvelteKitAuth({
    providers: [GitHub({ clientId: GOOGLE_ID, clientSecret: GOOGLE_SECRET })],
})