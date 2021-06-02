import { EventEmitter, Component, ElementRef, Input, Output, NgModule } from '@angular/core';
import { create } from 'nipplejs';

import * as ɵngcc0 from '@angular/core';
class NgxJoystickComponent {
    constructor(el) {
        this.el = el;
        this.move = new EventEmitter();
        // tslint:disable-next-line:no-output-native
        this.start = new EventEmitter();
        // tslint:disable-next-line:no-output-native
        this.end = new EventEmitter();
        this.dir = new EventEmitter();
        this.dirUp = new EventEmitter();
        this.dirDown = new EventEmitter();
        this.dirLeft = new EventEmitter();
        this.dirRight = new EventEmitter();
        this.plain = new EventEmitter();
        this.plainUp = new EventEmitter();
        this.plainDown = new EventEmitter();
        this.plainLeft = new EventEmitter();
        this.plainRight = new EventEmitter();
    }
    ngOnInit() {
        if (!this.options) {
            this.options = this.getDefaultOptions();
        }
        else {
            this.options.zone = this.el.nativeElement;
        }
        this.manager = create(this.options);
        this.setupEvents();
    }
    ngOnDestroy() {
        this.manager.destroy();
    }
    getDefaultOptions() {
        const options = {
            zone: this.el.nativeElement,
            mode: 'static',
            position: { left: '50%', top: '50%' },
            color: 'blue'
        };
        return options;
    }
    emitEvent(event, emitter) {
        const joystickEvent = { event: event.event, data: event.data };
        emitter.emit(joystickEvent);
    }
    setupEvents() {
        this.manager.on('move', (event, data) => { this.emitEvent({ event, data }, this.move); });
        this.manager.on('start', (event, data) => { this.emitEvent({ event, data }, this.start); });
        this.manager.on('end', (event, data) => { this.emitEvent({ event, data }, this.end); });
        this.manager.on('dir', (event, data) => { this.emitEvent({ event, data }, this.dir); });
        this.manager.on('dir:up', (event, data) => { this.emitEvent({ event, data }, this.dirUp); });
        this.manager.on('dir:down', (event, data) => { this.emitEvent({ event, data }, this.dirDown); });
        this.manager.on('dir:left', (event, data) => { this.emitEvent({ event, data }, this.dirLeft); });
        this.manager.on('dir:right', (event, data) => { this.emitEvent({ event, data }, this.dirRight); });
        this.manager.on('plain', (event, data) => { this.emitEvent({ event, data }, this.plain); });
        this.manager.on('plain:up', (event, data) => { this.emitEvent({ event, data }, this.plainUp); });
        this.manager.on('plain:down', (event, data) => { this.emitEvent({ event, data }, this.plainDown); });
        this.manager.on('plain:left', (event, data) => { this.emitEvent({ event, data }, this.plainLeft); });
        this.manager.on('plain:right', (event, data) => { this.emitEvent({ event, data }, this.plainRight); });
    }
}
NgxJoystickComponent.ɵfac = function NgxJoystickComponent_Factory(t) { return new (t || NgxJoystickComponent)(ɵngcc0.ɵɵdirectiveInject(ɵngcc0.ElementRef)); };
NgxJoystickComponent.ɵcmp = /*@__PURE__*/ ɵngcc0.ɵɵdefineComponent({ type: NgxJoystickComponent, selectors: [["ngx-joystick"]], inputs: { options: "options" }, outputs: { move: "move", start: "start", end: "end", dir: "dir", dirUp: "dirUp", dirDown: "dirDown", dirLeft: "dirLeft", dirRight: "dirRight", plain: "plain", plainUp: "plainUp", plainDown: "plainDown", plainLeft: "plainLeft", plainRight: "plainRight" }, decls: 1, vars: 0, consts: [["id", "static", 2, "width", "100%", "height", "100%"]], template: function NgxJoystickComponent_Template(rf, ctx) { if (rf & 1) {
        ɵngcc0.ɵɵelement(0, "div", 0);
    } }, encapsulation: 2 });
NgxJoystickComponent.ctorParameters = () => [
    { type: ElementRef }
];
NgxJoystickComponent.propDecorators = {
    options: [{ type: Input }],
    move: [{ type: Output }],
    start: [{ type: Output }],
    end: [{ type: Output }],
    dir: [{ type: Output }],
    dirUp: [{ type: Output }],
    dirDown: [{ type: Output }],
    dirLeft: [{ type: Output }],
    dirRight: [{ type: Output }],
    plain: [{ type: Output }],
    plainUp: [{ type: Output }],
    plainDown: [{ type: Output }],
    plainLeft: [{ type: Output }],
    plainRight: [{ type: Output }]
};
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && ɵngcc0.ɵsetClassMetadata(NgxJoystickComponent, [{
        type: Component,
        args: [{
                selector: 'ngx-joystick',
                template: `
  <div style="width: 100%; height: 100%" id="static"></div>
  `
            }]
    }], function () { return [{ type: ɵngcc0.ElementRef }]; }, { move: [{
            type: Output
        }], start: [{
            type: Output
        }], end: [{
            type: Output
        }], dir: [{
            type: Output
        }], dirUp: [{
            type: Output
        }], dirDown: [{
            type: Output
        }], dirLeft: [{
            type: Output
        }], dirRight: [{
            type: Output
        }], plain: [{
            type: Output
        }], plainUp: [{
            type: Output
        }], plainDown: [{
            type: Output
        }], plainLeft: [{
            type: Output
        }], plainRight: [{
            type: Output
        }], options: [{
            type: Input
        }] }); })();

class NgxJoystickModule {
}
NgxJoystickModule.ɵfac = function NgxJoystickModule_Factory(t) { return new (t || NgxJoystickModule)(); };
NgxJoystickModule.ɵmod = /*@__PURE__*/ ɵngcc0.ɵɵdefineNgModule({ type: NgxJoystickModule });
NgxJoystickModule.ɵinj = /*@__PURE__*/ ɵngcc0.ɵɵdefineInjector({ imports: [[]] });
(function () { (typeof ngDevMode === "undefined" || ngDevMode) && ɵngcc0.ɵsetClassMetadata(NgxJoystickModule, [{
        type: NgModule,
        args: [{
                declarations: [NgxJoystickComponent],
                imports: [],
                exports: [NgxJoystickComponent]
            }]
    }], null, null); })();
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && ɵngcc0.ɵɵsetNgModuleScope(NgxJoystickModule, { declarations: [NgxJoystickComponent], exports: [NgxJoystickComponent] }); })();

/*
 * Public API Surface of ngx-joystick
 */

/**
 * Generated bundle index. Do not edit.
 */

export { NgxJoystickComponent, NgxJoystickModule };

//# sourceMappingURL=ngx-joystick.js.map