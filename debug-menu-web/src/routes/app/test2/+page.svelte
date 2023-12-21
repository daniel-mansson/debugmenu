<script lang="ts">
	import { InstanceConnection } from '$lib/InstanceConnection';
	import { parseAsyncApi } from '$lib/asyncApiHelpers';
	import ButtonContainer from '$lib/components/ButtonContainer.svelte';
	import Parser from '@asyncapi/parser';
	import { onMount } from 'svelte';

	onMount(async () => {
		let asyncApiYaml = `
  asyncapi: '2.6.0'
  info:
    title: Example AsyncAPI specification
    version: '0.1.0'
  channels:
    example-channel:
      subscribe:
        tags:
          - name: log
        message:
          payload:
            type: object
            properties:
              exampleField:
                type: string
              exampleNumber:
                type: number
              exampleDate:
                type: string
                format: date-time
    gameplay/spawn:
      publish:
        description: asdf
        tags:
          - name: button
        message:
          payload:
            type: object
            properties:
              exampleField:
                type: string
                description: This is an example text field
              exampleNumber:
                type: number
              exampleDate:
                type: string
                format: date-time
    gameplay/teleport:
      publish:
        description: asdf
        tags:
          - name: button
        message:
          payload:
            type: object
            properties:
              exampleField:
                type: string
                description: This is an example text field
              exampleNumber:
                type: number
              exampleDate:
                type: string
                format: date-time
    gameplay/restart:
      publish:
        tags:
          - name: button
`;

		let api = await parseAsyncApi(asyncApiYaml);

		buttons = api.buttons!;
	});

	let buttons: any[] = [];
</script>

<ButtonContainer channel="gameplay" {buttons} on:clicked={(r) => console.log(r.detail)} />
