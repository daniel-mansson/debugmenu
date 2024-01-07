<script lang="ts">
    import { Button } from "$lib/components/ui/button";
    import * as Card from "$lib/components/ui/card";
    import * as Select from "$lib/components/ui/select";
    import { Input } from "$lib/components/ui/input";
    import { Label } from "$lib/components/ui/label";
    const frameworks = [
      {
        value: "sveltekit",
        label: "SvelteKit"
      },
      {
        value: "next",
        label: "Next.js"
      },
      {
        value: "astro",
        label: "Astro"
      },
      {
        value: "nuxt",
        label: "Nuxt.js"
      }
    ];
  </script>
  <Card.Root class="min-w-96">
    <Card.Header>
      <Card.Title>Add XP</Card.Title>
      <Card.Description>Add xp to the current unit</Card.Description>
    </Card.Header>
    <Card.Content>
      <form>
        <div class="grid w-full items-center gap-4">
          <div class="flex flex-col space-y-1.5">
            <Label for="amount">Amount</Label>
            <Input id="amount" type="number" placeholder="0" />
          </div>
        </div>
      </form>
    </Card.Content>
    <Card.Footer class="flex justify-between">
      <Button variant="outline" class="active:outline active:outline-1">Add XP</Button>
    </Card.Footer>
  </Card.Root>