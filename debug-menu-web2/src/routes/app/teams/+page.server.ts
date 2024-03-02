import { DebugMenuBackend } from '$lib/backend/backend.js';
import { fail } from '@sveltejs/kit';
import { superValidate } from 'sveltekit-superforms/server';
import { z } from 'zod';

const schema = z.object({
    name: z.string().min(3).max(16)
});

export const load = (async () => {
    // Server API:
    const form = await superValidate(schema);

    // Unless you throw, always return { form } in load and form actions.
    return { form };
});

export const actions = {
    default: async ({ request, locals, fetch }) => {
        console.log('POST r', request);

        const form = await superValidate(request, schema);
        console.log('POST', form);

        // Convenient validation check:
        if (!form.valid) {
            // Again, return { form } and things will just work.
            return fail(400, { form });
        }

        if (locals.jwt && locals.user?.id) {
            console.log('creating')
            let result = await DebugMenuBackend(fetch, locals.jwt).createTeam(form.data.name, locals.user.id)
            console.log('res: ' + result.status)

        }
        // Yep, return { form } here too
        return { form };
    }
};