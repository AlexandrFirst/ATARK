import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MainContentComponent } from './mainContent.component';
import { RouterModule } from '@angular/router';


import { AccountComponent } from 'src/app/Modules/mainContent/Account/Account.component';
import { RoomComponent } from 'src/app/Modules/mainContent/Room/Room.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { NgxPaginationModule } from 'ngx-pagination';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { AdminGuard } from 'src/app/route-guards/admin.guard';
import { MainContentRouterModule } from 'src/app/Routers/mainContentRouter/mainContentRouter.module';
import { AdminPanelComponent } from './AdminPanel/AdminPanel.component';
import { FloorComponent } from './Floor/Floor.component';
import { BuildingsComponent } from './Buildings/Buildings.component';
import { BackupListComponent } from './backup-list/backup-list.component';
import { BuildingComponent } from './Building/Building.component';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxPaginationModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MainContentRouterModule
  ],
  declarations: [
    AdminPanelComponent,
    MainContentComponent,
    RoomComponent,
    FloorComponent,
    AccountComponent,
    BuildingsComponent,
    BuildingComponent,
    BackupListComponent
  ],
  providers: [
    DatePipe
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class MainContentModule { }
