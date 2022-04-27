import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';
import { ShareViewEditComponent } from './share-view-edit.component';
import { ShareViewEditNavMenuComponent } from './components/nav-menu/share-view-edit-nav-menu.component';
import { ShareViewEditDetailsComponent } from './pages/details/share-view-edit-details.component';
import { ShareViewEditReadmeComponent } from './pages/readme/share-view-edit-readme.component';


const routes: Routes = [
  { path: '', redirectTo: 'details' },
  { path: 'details', component: ShareViewEditDetailsComponent },
  { path: 'readme', component: ShareViewEditReadmeComponent }
]

@NgModule({
  declarations: [
    ShareViewEditComponent,
    ShareViewEditNavMenuComponent,
    ShareViewEditDetailsComponent,
    ShareViewEditReadmeComponent
  ],
  imports: [
    MaterialModule,
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
  ]
})
export class ShareViewEditModule { }
