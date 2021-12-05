import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginComponent } from 'src/app/Components/Login/Login.component';
import { MainContentGuard } from 'src/app/route-guards/mainContent.guard';
import { MainContentComponent } from 'src/app/Modules/mainContent/mainContent.component';
import { LoginGuard } from 'src/app/route-guards/login.guard';

@NgModule({
  imports: [
    RouterModule.forRoot([
      {
        path: 'entrance',
        component: LoginComponent,
        canActivate: [MainContentGuard]
      },
      {
        path: 'main', component: MainContentComponent,
        loadChildren: () => import('../../Modules/mainContent/mainContent.module').then(m => m.MainContentModule),
        canActivateChild: [LoginGuard]
      },
      { path: '', redirectTo: '/entrance', pathMatch: 'full' },
      { path: '**', redirectTo: '/entrance', pathMatch: 'full' },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class AppRouterModule { }
