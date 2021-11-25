import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { LeafletModule } from '@asymmetrik/ngx-leaflet';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { WorldMapComponent } from './Components/worldMap/worldMap.component';
import { MyMapComponent } from './Components/myMap/myMap.component';
import { LoginComponent } from './Components/Login/Login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ToastrModule } from 'ngx-toastr';
import { LoaderComponent } from './Components/Loader/Loader.component';
import { FooterComponent } from './Components/footer/footer.component';
import { RouterModule } from '@angular/router';
import { AddHeaderInterceptor } from './Interceptors/addHeadersInterceptor';
import { LoginGuard } from './route-guards/login.guard';
import { MainContentGuard } from './route-guards/mainContent.guard';
import { MainContentComponent } from './Modules/mainContent/mainContent.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { BuildingDialogComponent } from './Components/building-dialog/building-dialog.component';
import { CompartmentAddDialogComponent } from './Components/compartment-add-dialog/compartment-add-dialog.component';
import { ResponsibleUserAddDialogComponent } from './Components/responsible-user-add-dialog/responsible-user-add-dialog.component';
import { PositionInputDialogComponent } from './Components/position-input-dialog/position-input-dialog.component';
import { FloorAddDialogComponent } from './Components/floor-add-dialog/floor-add-dialog.component';
import { NgxQRCodeModule } from '@techiediaries/ngx-qrcode';
import { QrCodeDialogComponent } from './Components/qr-code-dialog/qr-code-dialog.component';
import {NgxPrintModule} from 'ngx-print';
import { TestDialogComponent } from './Components/test-dialog/test-dialog.component';
import { AddIotDialogComponent } from './Components/add-iot-dialog/add-iot-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';

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
    AddIotDialogComponent
  ],
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    BrowserModule,
    LeafletModule,
    NgbModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatDialogModule,
    NgxPaginationModule,
    NgxQRCodeModule,
    NgxPrintModule,
    RouterModule.forRoot([
      {
        path: 'entrance',
        component: LoginComponent,
        canActivate: [MainContentGuard]
      },
      {
        path: 'main', component: MainContentComponent,
        loadChildren: () => import('./Modules/mainContent/mainContent.module').then(m => m.MainContentModule),
        canActivateChild: [LoginGuard]
      },
      { path: '', redirectTo: '/entrance', pathMatch: 'full' },
      { path: '**', redirectTo: '/entrance', pathMatch: 'full' },
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AddHeaderInterceptor, multi: true }
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
