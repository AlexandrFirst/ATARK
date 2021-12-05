import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AccountComponent } from 'src/app/Modules/mainContent/Account/Account.component';
import { AdminPanelComponent } from 'src/app/Modules/mainContent/AdminPanel/AdminPanel.component';
import { BuildingComponent } from 'src/app/Modules/mainContent/Building/Building.component';
import { BuildingsComponent } from 'src/app/Modules/mainContent/Buildings/Buildings.component';
import { FloorComponent } from 'src/app/Modules/mainContent/Floor/Floor.component';

import { RoomComponent } from 'src/app/Modules/mainContent/Room/Room.component';
import { AdminGuard } from 'src/app/route-guards/admin.guard';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: 'room/:Id', component: RoomComponent },
      { path: 'floor/:Id', component: FloorComponent },
      { path: 'account', component: AccountComponent },
      { path: 'buildings', component: BuildingsComponent },
      { path: 'buildings/:buildingId', component: BuildingComponent },
      { path: 'adminPanel', component: AdminPanelComponent, canActivate: [AdminGuard] },
      { path: '', redirectTo: 'account', pathMatch: 'full' },
      { path: '**', redirectTo: 'account', pathMatch: 'full' }
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class MainContentRouterModule { }
