import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MainContentComponent } from './mainContent.component';
import { RouterModule } from '@angular/router';
import { BuildingComponent } from 'src/app/Components/Building/Building.component';
import { BuildingsComponent } from 'src/app/Components/Buildings/Buildings.component';
import { AccountComponent } from 'src/app/Components/Account/Account.component';
import { RoomComponent } from 'src/app/Components/Room/Room.component';
import { FloorComponent } from 'src/app/Components/Floor/Floor.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { NgxPaginationModule } from 'ngx-pagination';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { AdminPanelComponent } from 'src/app/Components/AdminPanel/AdminPanel.component';
import { AdminGuard } from 'src/app/route-guards/admin.guard';
import { MainContentRouterModule } from 'src/app/Routers/mainContentRouter/mainContentRouter.module';
import { BackupListComponent } from 'src/app/Components/backup-list/backup-list.component';

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
