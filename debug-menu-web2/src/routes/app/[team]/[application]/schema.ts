import { z } from 'zod';

export const schema = z.object({
    name: z.string().min(3).max(16),
    description: z.string().max(150),
    applicationId: z.number()
});

export type FormSchema = typeof schema;