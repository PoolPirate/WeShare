import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ReactiveFormsModule } from '@angular/forms';
import { ShareViewSettingsComponent } from './share-view-settings.component';
import { ShareViewSettingsDetailsComponent } from './pages/details/share-view-settings-details.component';
import { ShareViewSettingsNavMenuComponent } from './components/nav-menu/share-view-settings-nav-menu.component';
import { ShareViewSettingsProcessingComponent } from './pages/processing/share-view-settings-processing.component';
import { ShareViewSettingsCriticalComponent } from './pages/critical/share-view-settings-critical.component';
import { MaterialModule } from '../../../material/material.module';
import { SharedModule } from '../../../../shared/shared.module';

const routes: Routes = [
  { path: '', redirectTo: 'details' },
  { path: 'details', component: ShareViewSettingsDetailsComponent },
  { path: 'processing', component: ShareViewSettingsProcessingComponent },
  { path: 'critical', component: ShareViewSettingsCriticalComponent }
]

@NgModule({
  declarations: [
    ShareViewSettingsComponent,
    ShareViewSettingsNavMenuComponent,
    ShareViewSettingsDetailsComponent,
    ShareViewSettingsProcessingComponent,
    ShareViewSettingsCriticalComponent,
  ],
  imports: [
    MaterialModule,
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
  ],
  providers: [

  ]
})
export class ShareViewSettingsModule { }
