import { worker } from "cluster";

worker.precaching.precacheAndRoute(self.__precacheManifest || []);
fetch().then(() => {
    console.log("fetched");
});