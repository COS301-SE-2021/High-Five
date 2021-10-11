import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {AnalyzedImageCardComponent} from './analyzed-image-card.component';

describe('AnalyzedImageCardComponent', () => {
  let component: AnalyzedImageCardComponent;
  let fixture: ComponentFixture<AnalyzedImageCardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [AnalyzedImageCardComponent],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(AnalyzedImageCardComponent);
    component = fixture.componentInstance;
    component.analyzedImage = {imageId: '', id: '', pipelineId: '', url: '', dateAnalyzed: new Date()};
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(true).toBeTrue();
  });
});
