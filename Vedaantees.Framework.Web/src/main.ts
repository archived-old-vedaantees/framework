import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { HostModule } from  './host/host.module';
import { environment } from './environments/environment';
import 'hammerjs';

if (environment.production)  enableProdMode();

platformBrowserDynamic().bootstrapModule(HostModule)
                        .catch(err => console.log(err));