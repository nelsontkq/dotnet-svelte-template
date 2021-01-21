import type { RouteDefinition } from "svelte-spa-router";
import Home from "./routes/Home.svelte";
import NotFound from "./routes/NotFound.svelte";

let routes = {
  "/": Home,
  "*": NotFound,
};

export default routes as RouteDefinition;
