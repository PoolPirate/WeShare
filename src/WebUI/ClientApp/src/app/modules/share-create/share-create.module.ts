import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ShareCreateComponent } from './share-create.component';
import { BasicShareCreateComponent } from './pages/basic/basic-share-create.component';
import { ReadmeShareCreateComponent } from './pages/readme/readme-share-create.component';

const routes: Routes = [
  { path: '', component: ShareCreateComponent },
]

@NgModule({
  declarations: [
    ShareCreateComponent,
    BasicShareCreateComponent,
    ReadmeShareCreateComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
  ]
})
export class ShareCreateModule { }
