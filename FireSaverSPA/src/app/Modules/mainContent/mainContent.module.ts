import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainContentComponent } from './mainContent.component';
import { RouterModule } from '@angular/router';
import { StartMainPageComponent } from 'src/app/Components/startMainPage/startMainPage.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: '', component: StartMainPageComponent },
      { path: '**', component: StartMainPageComponent },
    ])
  ],
  declarations: [MainContentComponent]
})
export class MainContentModule { }
