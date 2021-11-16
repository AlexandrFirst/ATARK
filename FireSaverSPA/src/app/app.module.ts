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
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ToastrModule } from 'ngx-toastr';
import { LoaderComponent } from './Components/Loader/Loader.component';
import { FooterComponent } from './Components/footer/footer.component';

@NgModule({
  declarations: [		
    AppComponent,
      MyMapComponent,
      WorldMapComponent,
      LoginComponent,
      LoaderComponent,
      FooterComponent
   ],
  imports: [
    CommonModule,
    BrowserAnimationsModule, 
    ToastrModule.forRoot(),
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
