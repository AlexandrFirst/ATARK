import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { LeafletModule } from '@asymmetrik/ngx-leaflet';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { WorldMapComponent } from './Components/worldMap/worldMap.component';
import { MyMapComponent } from './Components/myMap/myMap.component';

@NgModule({
  declarations: [		
    AppComponent,
      MyMapComponent,
      WorldMapComponent
   ],
  imports: [
    BrowserModule,
    LeafletModule,
    NgbModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
