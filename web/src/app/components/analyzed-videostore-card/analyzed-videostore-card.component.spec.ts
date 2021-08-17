import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { AnalyzedVideostoreCardComponent } from './analyzed-videostore-card.component';

describe('AnalyzedVideostoreCardComponent', () => {
  let component: AnalyzedVideostoreCardComponent;
  let fixture: ComponentFixture<AnalyzedVideostoreCardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ AnalyzedVideostoreCardComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(AnalyzedVideostoreCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
