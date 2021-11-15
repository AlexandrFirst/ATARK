import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { LeafletModule } from '@asymmetrik/ngx-leaflet';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { WorldMapComponent } from './Components/worldMap/worldMap.component';
import { MyMapComponent } from './Components/myMap/myMap.component';
import { LoginComponent } from './Components/Login/Login.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [		
    AppComponent,
      MyMapComponent,
      WorldMapComponent,
      LoginComponent
   ],
  imports: [
    BrowserModule,
    LeafletModule,
    NgbModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
