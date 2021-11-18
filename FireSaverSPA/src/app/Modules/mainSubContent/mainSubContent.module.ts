import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainSubContentComponent } from './mainSubContent.component';
import { RouterModule } from '@angular/router';
import { BuildingComponent } from 'src/app/Components/Building/Building.component';
import { BuildingsComponent } from 'src/app/Components/Buildings/Buildings.component';
import { AccountComponent } from 'src/app/Components/Account/Account.component';
import { RoomComponent } from 'src/app/Components/Room/Room.component';
import { FloorComponent } from 'src/app/Components/Floor/Floor.component';
import { TestComponent } from 'src/app/Components/Test/Test.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: 'compartment/:compartmentId/test', component: TestComponent },
      { path: 'room/:roomId', component: RoomComponent },
      { path: 'floor/:floorId', component: FloorComponent },
      { path: 'account', component: AccountComponent },
      { path: 'buildings', component: BuildingsComponent },
      { path: 'buildings/:buildingId', component: BuildingComponent }
    ])
  ],
  declarations: [
    MainSubContentComponent,
    BuildingComponent,
    BuildingsComponent,
    AccountComponent,
    RoomComponent,
    FloorComponent,
    TestComponent]
})
export class MainSubContentModule { }
