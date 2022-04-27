import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '../../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';
import { PostViewComponent } from './post-view.component';
import { PostViewNavMenuComponent } from './components/nav-menu/post-view-nav-menu.component';

const routes: Routes = [
  { path: '', redirectTo: 'headers' },
  { path: 'headers' },
  { path: 'payload' }
]

@NgModule({
  declarations: [
    PostViewNavMenuComponent,
    PostViewComponent,
  ],
  imports: [
    MaterialModule,
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
  ]
})
export class PostViewModule { }
