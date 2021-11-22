import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MainContentComponent } from './mainContent.component';
import { RouterModule } from '@angular/router';
import { BuildingComponent } from 'src/app/Components/Building/Building.component';
import { BuildingsComponent } from 'src/app/Components/Buildings/Buildings.component';
import { AccountComponent } from 'src/app/Components/Account/Account.component';
import { RoomComponent } from 'src/app/Components/Room/Room.component';
import { FloorComponent } from 'src/app/Components/Floor/Floor.component';
import { TestComponent } from 'src/app/Components/Test/Test.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {MatDialogModule} from '@angular/material/dialog';
import {NgxPaginationModule} from 'ngx-pagination';
import {MatPaginatorModule} from '@angular/material/paginator';
import {MatProgressBarModule} from '@angular/material/progress-bar'; 

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    NgxPaginationModule,
    MatPaginatorModule,
    MatProgressBarModule,
    RouterModule.forChild([
      { path: 'compartment/:compartmentId/test', component: TestComponent },
      { path: 'room/:roomId', component: RoomComponent },
      { path: 'floor/:floorId', component: FloorComponent },
      { path: 'account', component: AccountComponent },
      { path: 'buildings', component: BuildingsComponent },
      { path: 'buildings/:buildingId', component: BuildingComponent },
      {path: '',  redirectTo: 'account', pathMatch: 'full'},
      {path: '**',  redirectTo: 'account', pathMatch: 'full'}
    ])
  ],
  declarations: [
    MainContentComponent,
    TestComponent,
    RoomComponent,
    FloorComponent,
    AccountComponent,
    BuildingsComponent,
    BuildingComponent
  ],
  providers:[
    DatePipe
  ],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class MainContentModule { }
