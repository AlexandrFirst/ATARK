import { CUSTOM_ELEMENTS_SCHEMA, LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { LeafletModule } from '@asymmetrik/ngx-leaflet';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { HttpClientJsonpModule, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { WorldMapComponent } from './Components/worldMap/worldMap.component';
import { MyMapComponent } from './Components/myMap/myMap.component';
import { LoginComponent } from './Components/Login/Login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ToastrModule } from 'ngx-toastr';

import { RouterModule } from '@angular/router';
import { AddHeaderInterceptor } from './Interceptors/addHeadersInterceptor';
import { LoginGuard } from './route-guards/login.guard';
import { MainContentGuard } from './route-guards/mainContent.guard';
import { MainContentComponent } from './Modules/mainContent/mainContent.component';
import { NgxPaginationModule } from 'ngx-pagination';

import { NgxQRCodeModule } from '@techiediaries/ngx-qrcode';

import {NgxPrintModule} from 'ngx-print';
import { TestDialogComponent } from './Modules/mainContent/test-dialog/test-dialog.component';

import { MatDialogModule } from '@angular/material/dialog';
import { registerLocaleData } from '@angular/common';
import localeUk from '@angular/common/locales/uk'
import { AppRouterModule } from './Routers/appRouter/appRouter.module';
import { LoaderComponent } from './Modules/mainContent/Loader/Loader.component';
import { FooterComponent } from './Components/Footer/footer.component';
import { BuildingDialogComponent } from './Modules/mainContent/building-dialog/building-dialog.component';
import { CompartmentAddDialogComponent } from './Modules/mainContent/compartment-add-dialog/compartment-add-dialog.component';
import { ResponsibleUserAddDialogComponent } from './Modules/mainContent/responsible-user-add-dialog/responsible-user-add-dialog.component';
import { PositionInputDialogComponent } from './Modules/mainContent/position-input-dialog/position-input-dialog.component';
import { FloorAddDialogComponent } from './Modules/mainContent/floor-add-dialog/floor-add-dialog.component';
import { QrCodeDialogComponent } from './Modules/mainContent/qr-code-dialog/qr-code-dialog.component';
import { AddIotDialogComponent } from './Modules/mainContent/add-iot-dialog/add-iot-dialog.component';
import { ShelterDialogComponent } from './Modules/mainContent/shelter-dialog/shelter-dialog.component';

registerLocaleData(localeUk, 'uk')

@NgModule({
  declarations: [
    AppComponent,
    MyMapComponent,
    WorldMapComponent,
    LoginComponent,
    LoaderComponent,
    FooterComponent,
    BuildingDialogComponent,
    CompartmentAddDialogComponent,
    ResponsibleUserAddDialogComponent,
    PositionInputDialogComponent,
    FloorAddDialogComponent,
    QrCodeDialogComponent,
    TestDialogComponent,
    AddIotDialogComponent,
    ShelterDialogComponent
  ],
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    BrowserModule,
    LeafletModule,
    NgbModule,
    HttpClientModule,
    HttpClientJsonpModule,
    ReactiveFormsModule,
    MatDialogModule,
    NgxPaginationModule,
    NgxQRCodeModule,
    NgxPrintModule,
    AppRouterModule,
    
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AddHeaderInterceptor, multi: true }
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
