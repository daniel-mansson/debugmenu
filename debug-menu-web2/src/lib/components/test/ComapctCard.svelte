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

    var buttons = [
      {id: 1},
      {id: 2},
      {id: 3},
      {id: 4},
    ]
  </script>
  <Card.Root class="min-w-96" >
    <Card.Header >
      <Card.Title>Add XP</Card.Title>
      <Card.Description>Add xp to the current unit</Card.Description>
    </Card.Header>

    <Card.Footer class="grid grid-cols-3 gap-2 justify-between">
      <Button variant="outline" class="active:outline active:outline-1">Add XP...</Button>
      <Button variant="outline" class="active:outline active:outline-1">+20</Button>
      <Button variant="outline" class="active:outline active:outline-1">+100</Button>
      <Button variant="outline" class="active:outline active:outline-1">+300</Button>
    </Card.Footer>
  </Card.Root>