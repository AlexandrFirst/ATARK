import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';


if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));

document.write('<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyD9SY3tDElTa-oBJmrVehq6LYV3W42j5Wg"></script>')
