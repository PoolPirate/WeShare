import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { WeShareClient } from '../../../../services/weshareclient';
import { ShareData, ShareInfo } from '../../../../types/share-types';
import { ShareService } from '../../../share-view/services/shareservice';

@Component({
  selector: 'share-view-edit-details',
  templateUrl: './share-view-edit-details.component.html'
})
export class ShareViewEditDetailsComponent {
  shareData: ShareData;

  detailsForm: FormGroup;

  ranForName: string = "";
  ranForDescription = "";
  errorCode: number = 0;

  constructor(private weShareClient: WeShareClient,
    shareService: ShareService, formBuilder: FormBuilder)
  {
    this.shareData = shareService.shareData;

    this.detailsForm = formBuilder.group({
      name: [this.shareData.shareInfo.name, [Validators.required, Validators.minLength(3), Validators.maxLength(32)]],
      description: [this.shareData.shareInfo.description, [Validators.maxLength(256)]]
    });
  }

  updateDetails() {
    this.errorCode = 0;

    if (this.detailsForm.invalid) {
      this.detailsForm.markAllAsTouched();
      return;
    }

    const val = this.detailsForm.value;
    const name: string = val.name;
    const description: string = val.description;

    if (this.shareData.shareInfo.name == name && this.shareData.shareInfo.description == description) { return; }
    if (this.ranForName == name && this.ranForDescription == description) { return; }

    this.ranForName = name;
    this.ranForDescription = description;

    this.weShareClient.updateShare(this.shareData.shareInfo.id, name, description, null)
      .subscribe(response => {
        this.shareData.shareInfo.name = name;
        this.shareData.shareInfo.description = description;
        this.errorCode = 200;
      }, (error: HttpErrorResponse) => {
        this.errorCode = error.status;
      });
  }

  get name() {
    return this.detailsForm.get('name')!;
  }
  get description() {
    return this.detailsForm.get('description')!;
  }
}
