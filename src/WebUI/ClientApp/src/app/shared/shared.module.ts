import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ShareSnippetComponent } from './components/share-snippet/share-snippet.component';
import { MatIconModule } from '@angular/material/icon';
import { LikeButtonComponent } from './components/like-button/like-button.component';
import { MatButtonModule } from '@angular/material/button';
import { SubscriptionSnippetComponent } from './components/subscription-snippet/subscription-snippet.component';
import { PostSnippetComponent } from './components/post-snippet/post-snippet.component';
import { MatCardModule } from '@angular/material/card';
import { MatExpansionModule } from '@angular/material/expansion';
import { CreateButtonComponent } from './components/create-button/create-button.component';
import { PagedListHeaderComponent } from './components/paged-list-header/paged-list-header.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { HeaderPairComponent } from './components/header-pair/header-pair.component';
import { SharedLoadingDialog } from './dialogs/loading/loading.dialog.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { SharedConfirmDialog } from './dialogs/confirm/confirm.dialog';
import { MatMenuModule } from '@angular/material/menu';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogService } from '../../services/dialogservice';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatStepperModule } from '@angular/material/stepper';

@NgModule({
  declarations: [
    ShareSnippetComponent,
    LikeButtonComponent,
    SubscriptionSnippetComponent,
    PostSnippetComponent,
    CreateButtonComponent,
    PagedListHeaderComponent,
    HeaderPairComponent,

    SharedLoadingDialog,
    SharedConfirmDialog
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([]),

    FormsModule,
    ReactiveFormsModule,

    MatExpansionModule,
    MatButtonModule,
    MatCardModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatIconModule,
    MatMenuModule,
  ],
  exports: [
    LikeButtonComponent,
    CreateButtonComponent,

    SharedLoadingDialog,
    SharedConfirmDialog,

    ShareSnippetComponent,
    SubscriptionSnippetComponent,
    PostSnippetComponent,

    HeaderPairComponent,

    PagedListHeaderComponent,

    FormsModule,
    ReactiveFormsModule,

    CommonModule,

    MatExpansionModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatMenuModule,
    MatChipsModule,
  ]
})
export class SharedModule { }
