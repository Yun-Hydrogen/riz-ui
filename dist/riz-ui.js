import { openBlock as c, createElementBlock as u, createElementVNode as o, normalizeStyle as k, normalizeClass as T, createCommentVNode as C, toDisplayString as j, renderSlot as G, useSlots as me, computed as f, createBlock as se, Teleport as ue, createVNode as ne, Transition as ie, withCtx as le, withModifiers as A, mergeProps as he, ref as R, onMounted as Z, onBeforeUnmount as ee, useCssVars as de, Fragment as oe, renderList as ae, withKeys as ve, resolveDynamicComponent as pe, createTextVNode as fe, nextTick as re } from "vue";
function Qt(t, m = "_blank") {
  const e = document.createElement("a");
  e.href = t, e.target = m, e.rel = "noopener noreferrer", document.body.appendChild(e), e.click(), e.remove();
}
const U = (t, m) => {
  const e = t.__vccOpts || t;
  for (const [g, p] of m)
    e[g] = p;
  return e;
}, ye = { class: "ct-card" }, ge = { class: "ct-card-head" }, be = { class: "ct-card-head-text" }, ke = {
  key: 0,
  class: "ct-card-body"
}, $e = {
  __name: "Cntr_Card",
  props: {
    title: { type: String, default: "" },
    titleInstyle: { type: Object, default: () => ({}) },
    desc: { type: String, default: "" },
    descInstyle: { type: Object, default: () => ({}) },
    icon: { type: String, default: "" },
    iconInstyle: { type: Object, default: () => ({}) }
  },
  setup(t) {
    return (m, e) => (c(), u("div", ye, [
      o("div", ge, [
        t.icon ? (c(), u("div", {
          key: 0,
          class: "ct-card-icon",
          style: k(t.iconInstyle)
        }, [
          o("i", {
            class: T(t.icon)
          }, null, 2)
        ], 4)) : C("", !0),
        o("div", be, [
          t.title ? (c(), u("div", {
            key: 0,
            class: "ct-card-title",
            style: k(t.titleInstyle)
          }, j(t.title), 5)) : C("", !0),
          t.desc ? (c(), u("div", {
            key: 1,
            class: "ct-card-desc",
            style: k(t.descInstyle)
          }, j(t.desc), 5)) : C("", !0)
        ])
      ]),
      m.$slots.default ? (c(), u("div", ke, [
        G(m.$slots, "default", {}, void 0, !0)
      ])) : C("", !0)
    ]));
  }
}, Zt = /* @__PURE__ */ U($e, [["__scopeId", "data-v-ca4cde43"]]), xe = { class: "cfgr-head" }, we = { class: "cfgr-head-text" }, Se = {
  key: 0,
  class: "cfgr-slot-wrap"
}, Ve = {
  __name: "Cntr_Cfgrow",
  props: {
    label: { type: String, default: "" },
    labelInstyle: { type: Object, default: () => ({}) },
    hint: { type: String, default: "" },
    hintInstyle: { type: Object, default: () => ({}) },
    icon: { type: String, default: "" },
    iconInstyle: { type: Object, default: () => ({}) },
    stack: { type: Boolean, default: !1 },
    disabled: { type: Boolean, default: !1 }
  },
  setup(t) {
    return (m, e) => (c(), u("div", {
      class: T(["cfgr-row", { "cfgr-stack": t.stack, "cfgr-disabled": t.disabled }])
    }, [
      o("div", xe, [
        t.icon ? (c(), u("div", {
          key: 0,
          class: "cfgr-icon",
          style: k(t.iconInstyle)
        }, [
          o("i", {
            class: T(t.icon)
          }, null, 2)
        ], 4)) : C("", !0),
        o("div", we, [
          t.label ? (c(), u("div", {
            key: 0,
            class: "cfgr-label",
            style: k(t.labelInstyle)
          }, j(t.label), 5)) : C("", !0),
          t.hint ? (c(), u("div", {
            key: 1,
            class: "cfgr-hint",
            style: k(t.hintInstyle)
          }, j(t.hint), 5)) : C("", !0)
        ]),
        t.stack ? C("", !0) : (c(), u("div", {
          key: 1,
          class: T(["cfgr-slot-inner", { "cfgr-slot-disabled": t.disabled }])
        }, [
          G(m.$slots, "default", {}, void 0, !0)
        ], 2))
      ]),
      t.stack ? (c(), u("div", Se, [
        o("div", {
          class: T(["cfgr-slot-inner", { "cfgr-slot-disabled": t.disabled }])
        }, [
          G(m.$slots, "default", {}, void 0, !0)
        ], 2)
      ])) : C("", !0)
    ], 2));
  }
}, en = /* @__PURE__ */ U(Ve, [["__scopeId", "data-v-2ff8117d"]]), Me = { class: "inbx-content" }, Ce = {
  __name: "Cntr_Infobox",
  props: {
    type: { type: String, default: "info" },
    text: { type: String, default: "" },
    textInstyle: { type: Object, default: () => ({}) },
    icon: { type: String, default: "" },
    iconInstyle: { type: Object, default: () => ({}) },
    color: { type: String, default: "" }
  },
  setup(t) {
    const m = t, e = me(), g = f(() => !!e.default), p = f(() => !!m.text && g.value), r = {
      info: "fa-solid fa-circle-info",
      warn: "fa-solid fa-triangle-exclamation",
      error: "fa-solid fa-circle-exclamation"
    }, y = f(() => r[m.type] || r.info), x = { warn: "#e6a23c", error: "#e05555" }, S = { warn: "#fff7e6", error: "#fdecec" }, V = f(() => m.type === "info" ? m.color || "var(--rizui_themecolor)" : x[m.type] || "var(--rizui_themecolor)"), I = f(() => m.type === "info" ? `color-mix(in srgb, ${V.value} 10%, white)` : S[m.type] || `color-mix(in srgb, ${V.value} 10%, white)`), B = f(
      () => m.type === "info" ? { "--inbx-color": V.value, "--inbx-bg": I.value } : null
    );
    return (w, E) => (c(), u("div", {
      class: T(["inbx-root", `inbx-${t.type}`]),
      style: k(B.value)
    }, [
      o("div", {
        class: "inbx-icon",
        style: k(t.iconInstyle)
      }, [
        o("i", {
          class: T(t.icon || y.value)
        }, null, 2)
      ], 4),
      o("div", Me, [
        t.text ? (c(), u("div", {
          key: 0,
          class: "inbx-text",
          style: k(t.textInstyle)
        }, j(t.text), 5)) : C("", !0),
        g.value ? (c(), u("div", {
          key: 1,
          class: T(["inbx-slot", { "inbx-slot-stack": p.value }])
        }, [
          G(w.$slots, "default", {}, void 0, !0)
        ], 2)) : C("", !0)
      ])
    ], 6));
  }
}, tn = /* @__PURE__ */ U(Ce, [["__scopeId", "data-v-c6bb2832"]]), Ie = { class: "cdlg-header" }, Ee = {
  key: 0,
  class: "cdlg-body"
}, Le = {
  key: 1,
  class: "cdlg-footer"
}, Te = /* @__PURE__ */ Object.assign({ inheritAttrs: !1 }, {
  __name: "Ctnr_Dialog",
  props: {
    show: { type: Boolean, default: !1 },
    title: { type: String, default: "" },
    titleInstyle: { type: Object, default: () => ({}) },
    icon: { type: String, default: "" },
    iconInstyle: { type: Object, default: () => ({}) },
    color: { type: String, default: "" },
    teleport: { type: String, default: "body" },
    width: { type: String, default: "auto" },
    closable: { type: Boolean, default: !0 },
    maskClose: { type: Boolean, default: !0 }
  },
  emits: ["update:show"],
  setup(t, { emit: m }) {
    const e = t, g = m, p = f(() => e.teleport !== "body"), r = f(() => e.color || "var(--rizui_themecolor)"), y = f(() => ({
      width: e.width,
      "--cdlg-color": r.value
    }));
    function x() {
      g("update:show", !1);
    }
    function S() {
      e.maskClose && x();
    }
    return (V, I) => (c(), se(ue, { to: t.teleport }, [
      ne(ie, { name: "cdlg-pop" }, {
        default: le(() => [
          t.show ? (c(), u("div", {
            key: 0,
            class: T(["cdlg-overlay", { "cdlg-anchored": p.value }]),
            onClick: A(S, ["self"])
          }, [
            o("div", he({
              class: "cdlg-box",
              style: y.value
            }, V.$attrs), [
              o("div", Ie, [
                t.icon ? (c(), u("div", {
                  key: 0,
                  class: "cdlg-icon",
                  style: k(t.iconInstyle)
                }, [
                  o("i", {
                    class: T(t.icon)
                  }, null, 2)
                ], 4)) : C("", !0),
                o("span", {
                  class: "cdlg-title",
                  style: k(t.titleInstyle)
                }, j(t.title), 5),
                t.closable ? (c(), u("button", {
                  key: 1,
                  class: "cdlg-close",
                  onClick: x,
                  type: "button"
                }, [...I[0] || (I[0] = [
                  o("svg", {
                    viewBox: "0 0 24 24",
                    width: "12",
                    height: "12",
                    stroke: "currentColor",
                    "stroke-width": "2.5",
                    fill: "none"
                  }, [
                    o("path", { d: "M18 6L6 18M6 6l12 12" })
                  ], -1)
                ])])) : C("", !0)
              ]),
              V.$slots.default ? (c(), u("div", Ee, [
                G(V.$slots, "default", {}, void 0, !0)
              ])) : C("", !0),
              V.$slots.footer ? (c(), u("div", Le, [
                G(V.$slots, "footer", {}, void 0, !0)
              ])) : C("", !0)
            ], 16)
          ], 2)) : C("", !0)
        ]),
        _: 3
      })
    ], 8, ["to"]));
  }
}), nn = /* @__PURE__ */ U(Te, [["__scopeId", "data-v-7c4adc9e"]]), Be = { class: "tst-content" }, Re = {
  key: 1,
  class: "tst-bar"
}, _e = {
  __name: "Ctnr_Toast",
  props: {
    type: { type: String, default: "info" },
    title: { type: String, default: "" },
    hint: { type: String, default: "" },
    titleInstyle: { type: Object, default: () => ({}) },
    hintInstyle: { type: Object, default: () => ({}) },
    icon: { type: String, default: "" },
    iconInstyle: { type: Object, default: () => ({}) },
    countdown: { type: Number, default: 3 },
    closable: { type: Boolean, default: !0 },
    showBar: { type: Boolean, default: !0 },
    color: { type: String, default: "" },
    teleport: { type: String, default: "body" },
    width: { type: String, default: "" }
  },
  emits: ["close", "click"],
  setup(t, { expose: m, emit: e }) {
    const g = t, p = e, r = me(), y = f(() => !!r.default), x = f(() => (!!g.title || !!g.hint) && y.value), S = R(!0), V = R(typeof document < "u" ? L() : null), I = {
      info: "fa-solid fa-circle-info",
      warn: "fa-solid fa-triangle-exclamation",
      error: "fa-solid fa-circle-exclamation"
    }, B = f(() => I[g.type] || I.info), w = { warn: "#e6a23c", error: "#e05555" }, E = f(() => g.type === "info" ? g.color || "var(--rizui_themecolor)" : w[g.type] || "var(--rizui_themecolor)");
    function L() {
      const l = document.querySelector(g.teleport) || document.body;
      let s = document.getElementById("rz-toast-stack");
      if (s || (s = document.createElement("div"), s.id = "rz-toast-stack"), s.parentElement !== l && l.appendChild(s), !s.__rzBound) {
        s.__rzBound = !0;
        const n = (v) => {
          const X = [...s.children].filter((D) => D.classList.contains("tst-root")), N = /* @__PURE__ */ new Map();
          for (const D of X) N.set(D, D.getBoundingClientRect().top);
          let K = 0, J = 0;
          for (const D of X)
            D.style.transition = "none", D.style.top = K + "px", K += D.offsetHeight + 10, J = Math.max(J, D.offsetWidth);
          if (J > 0 && (s.style.width = J + "px"), v)
            for (const D of X) D.style.transition = "";
          else {
            s.offsetHeight;
            for (const D of X) {
              const O = N.get(D) - D.getBoundingClientRect().top;
              Math.abs(O) > 0.5 ? (D.style.transform = `translateY(${O}px)`, D.offsetHeight, D.style.transition = "transform 0.3s ease", D.style.transform = "", D.addEventListener("transitionend", () => {
                D.style.transition = "";
              }, { once: !0 })) : D.style.transition = "";
            }
          }
        };
        new MutationObserver((v) => {
          const X = v.some((N) => N.addedNodes.length);
          n(!!X);
        }).observe(s, { childList: !0 });
      }
      return s;
    }
    const _ = Date.now();
    let z = null;
    Z(() => {
      g.countdown > 0 && (z = setTimeout(F, g.countdown * 1e3));
    }), ee(() => {
      z && clearTimeout(z);
    });
    function F() {
      S.value && (S.value = !1);
    }
    function $() {
      const l = Math.round((Date.now() - _) / 100) / 10;
      p("close", l);
    }
    function a() {
      p("click");
    }
    return m({ close: F }), (l, s) => (c(), se(ue, { to: V.value }, [
      ne(ie, {
        name: "tst-pop",
        onAfterLeave: $
      }, {
        default: le(() => [
          S.value ? (c(), u("div", {
            key: 0,
            class: T(["tst-root", `tst-${t.type}`]),
            style: k({ "--tst-color": E.value, "--tst-countdown": t.countdown + "s", "--tst-width": t.width }),
            onClick: a
          }, [
            o("div", {
              class: "tst-icon",
              style: k(t.iconInstyle)
            }, [
              o("i", {
                class: T(t.icon || B.value)
              }, null, 2)
            ], 4),
            o("div", Be, [
              t.title ? (c(), u("div", {
                key: 0,
                class: "tst-title",
                style: k(t.titleInstyle)
              }, j(t.title), 5)) : C("", !0),
              t.hint ? (c(), u("div", {
                key: 1,
                class: "tst-hint",
                style: k(t.hintInstyle)
              }, j(t.hint), 5)) : C("", !0),
              y.value ? (c(), u("div", {
                key: 2,
                class: T(["tst-slot", { "tst-slot-stack": x.value }])
              }, [
                G(l.$slots, "default", {}, void 0, !0)
              ], 2)) : C("", !0)
            ]),
            t.closable ? (c(), u("button", {
              key: 0,
              class: "tst-close",
              onClick: A(F, ["stop"]),
              type: "button"
            }, [...s[0] || (s[0] = [
              o("svg", {
                viewBox: "0 0 24 24",
                width: "10",
                height: "10",
                stroke: "currentColor",
                "stroke-width": "3",
                fill: "none"
              }, [
                o("path", { d: "M18 6L6 18M6 6l12 12" })
              ], -1)
            ])])) : C("", !0),
            t.showBar ? (c(), u("div", Re, [
              o("div", {
                class: T(["tst-bar-inner", { "tst-bar-full": !(t.countdown > 0) }])
              }, null, 2)
            ])) : C("", !0)
          ], 6)) : C("", !0)
        ]),
        _: 3
      })
    ], 8, ["to"]));
  }
}, ln = /* @__PURE__ */ U(_e, [["__scopeId", "data-v-76740fdf"]]), Xe = ["checked", "disabled"], Oe = {
  __name: "Ctrl_Switch",
  props: {
    modelValue: { type: Boolean, default: !1 },
    color: { type: String, default: "" },
    disabled: { type: Boolean, default: !1 }
  },
  emits: ["update:modelValue"],
  setup(t) {
    const m = t, e = f(() => m.color || "var(--rizui_themecolor)"), g = f(() => ({
      background: m.modelValue ? e.value : void 0
    }));
    return (p, r) => (c(), u("label", {
      class: T(["csw-root", { "csw-disabled": t.disabled }])
    }, [
      o("input", {
        type: "checkbox",
        checked: t.modelValue,
        disabled: t.disabled,
        onChange: r[0] || (r[0] = (y) => p.$emit("update:modelValue", y.target.checked))
      }, null, 40, Xe),
      o("span", {
        class: "csw-track",
        style: k(g.value)
      }, null, 4)
    ], 2));
  }
}, on = /* @__PURE__ */ U(Oe, [["__scopeId", "data-v-4e926be9"]]), Ne = { class: "csl-value" }, De = 400, ze = 60, je = {
  __name: "Ctrl_Slider",
  props: {
    modelValue: { type: Number, default: 0 },
    min: { type: Number, default: 0 },
    max: { type: Number, default: 100 },
    step: { type: Number, default: 1 },
    color: { type: String, default: "" },
    display: { type: String, default: "" },
    displayValue: { type: String, default: "" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: m }) {
    const e = t, g = m, p = R(null), r = f(() => e.color || "var(--rizui_themecolor)"), y = f(() => {
      const a = e.max - e.min;
      return a <= 0 ? 0 : Math.max(0, Math.min(100, (e.modelValue - e.min) / a * 100));
    }), x = f(() => ({
      background: `linear-gradient(to right, color-mix(in srgb, ${r.value} 20%, transparent) ${y.value}%, transparent ${y.value}%)`,
      borderColor: r.value
    }));
    function S(a) {
      return e.step < 1 ? parseFloat(a) : parseInt(a);
    }
    function V(a) {
      const l = e.modelValue + a * e.step, s = Math.max(e.min, Math.min(e.max, l)), n = S(s.toFixed(10));
      n !== e.modelValue && g("update:modelValue", n);
    }
    const I = R(!1);
    function B(a) {
      if (!p.value) return e.modelValue;
      const l = p.value.getBoundingClientRect(), s = Math.max(0, Math.min(1, (a - l.left) / l.width)), n = e.min + s * (e.max - e.min), b = Math.round(n / e.step) * e.step;
      return S(Math.max(e.min, Math.min(e.max, b)).toFixed(10));
    }
    function w(a) {
      var b, v;
      I.value = !0;
      const l = a.target, s = l.closest(".csl-minus"), n = l.closest(".csl-plus");
      if (s)
        V(-1), z(-1);
      else if (n)
        V(1), z(1);
      else {
        const X = B(a.clientX || ((v = (b = a.touches) == null ? void 0 : b[0]) == null ? void 0 : v.clientX) || 0);
        X !== e.modelValue && g("update:modelValue", X);
      }
    }
    function E(a) {
      var s, n;
      if (!I.value) return;
      F();
      const l = B(a.clientX || ((n = (s = a.touches) == null ? void 0 : s[0]) == null ? void 0 : n.clientX) || 0);
      l !== e.modelValue && g("update:modelValue", l);
    }
    function L() {
      I.value = !1, F();
    }
    const _ = R(null);
    function z(a) {
      F(), _.value = setTimeout(() => {
        _.value = setInterval(() => V(a), ze);
      }, De);
    }
    function F() {
      _.value && (clearTimeout(_.value), clearInterval(_.value), _.value = null);
    }
    Z(() => {
      document.addEventListener("mousemove", E), document.addEventListener("mouseup", L), document.addEventListener("touchmove", E, { passive: !1 }), document.addEventListener("touchend", L);
    }), ee(() => {
      document.removeEventListener("mousemove", E), document.removeEventListener("mouseup", L), document.removeEventListener("touchmove", E), document.removeEventListener("touchend", L), F();
    });
    const $ = f(() => {
      if (e.displayValue) return e.displayValue;
      const a = e.step < 1 ? e.modelValue.toFixed(1) : e.modelValue;
      return e.display ? `${a}${e.display}` : String(a);
    });
    return (a, l) => (c(), u("div", {
      ref_key: "rootRef",
      ref: p,
      class: T(["csl-root", { "csl-dragging": I.value }]),
      style: k(x.value),
      onMousedown: A(w, ["left"]),
      onTouchstart: A(w, ["prevent"])
    }, [
      l[0] || (l[0] = o("button", { class: "csl-btn csl-minus" }, "−", -1)),
      o("span", Ne, j($.value), 1),
      l[1] || (l[1] = o("button", { class: "csl-btn csl-plus" }, "+", -1))
    ], 38));
  }
}, an = /* @__PURE__ */ U(je, [["__scopeId", "data-v-f29324c6"]]), Fe = {
  key: 0,
  class: "cdd-drop"
}, Ue = ["onClick"], Pe = {
  __name: "Ctrl_Dropdown",
  props: {
    modelValue: { type: [String, Number], default: "" },
    options: { type: Array, default: () => [] },
    color: { type: String, default: "" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: m }) {
    de((w) => ({
      ec7c64f6: y.value
    }));
    const e = t, g = m, p = R(null), r = R(!1), y = f(() => e.color || "var(--rizui_themecolor)"), x = f(() => {
      const w = e.options.find((E) => E.value === e.modelValue);
      return w ? w.label : String(e.modelValue || "");
    }), S = f(() => ({
      color: y.value,
      background: `color-mix(in srgb, ${y.value} 8%, transparent)`
    }));
    function V() {
      r.value = !r.value;
    }
    function I(w) {
      g("update:modelValue", w), r.value = !1;
    }
    function B(w) {
      p.value && !p.value.contains(w.target) && (r.value = !1);
    }
    return Z(() => {
      document.addEventListener("mousedown", B, !0), document.addEventListener("touchstart", B, !0);
    }), ee(() => {
      document.removeEventListener("mousedown", B, !0), document.removeEventListener("touchstart", B, !0);
    }), (w, E) => (c(), u("div", {
      ref_key: "rootRef",
      ref: p,
      class: T(["cdd-root", { "cdd-open": r.value }])
    }, [
      o("button", {
        class: "cdd-trigger",
        onClick: V,
        type: "button"
      }, [
        o("span", null, j(x.value), 1),
        E[0] || (E[0] = o("svg", {
          class: "cdd-arrow",
          viewBox: "0 0 12 7",
          width: "10",
          height: "6"
        }, [
          o("path", {
            d: "M1 1l5 5 5-5",
            stroke: "currentColor",
            "stroke-width": "1.5",
            fill: "none",
            "stroke-linecap": "round"
          })
        ], -1))
      ]),
      ne(ie, { name: "cdd-pop" }, {
        default: le(() => [
          r.value ? (c(), u("div", Fe, [
            (c(!0), u(oe, null, ae(t.options, (L) => (c(), u("button", {
              key: L.value,
              class: T(["cdd-opt", { "cdd-active": L.value === t.modelValue }]),
              style: k(L.value === t.modelValue ? S.value : {}),
              onClick: (_) => I(L.value),
              type: "button"
            }, j(L.label), 15, Ue))), 128))
          ])) : C("", !0)
        ]),
        _: 1
      })
    ], 2));
  }
}, sn = /* @__PURE__ */ U(Pe, [["__scopeId", "data-v-5d296197"]]), Ae = ["value", "placeholder", "disabled", "readonly"], He = ["type", "value", "placeholder", "disabled", "readonly"], Ye = {
  __name: "Inpt_Text",
  props: {
    modelValue: { type: [String, Number], default: "" },
    type: { type: String, default: "text" },
    placeholder: { type: String, default: "" },
    color: { type: String, default: "" },
    plaintext: { type: Boolean, default: !1 },
    disabled: { type: Boolean, default: !1 },
    readonly: { type: Boolean, default: !1 }
  },
  emits: ["update:modelValue", "enter"],
  setup(t, { emit: m }) {
    const e = t, g = m, p = f(() => ({
      "--inpt-color": e.color || "var(--rizui_themecolor)"
    }));
    function r(y) {
      g("enter", e.modelValue), y.target.blur();
    }
    return (y, x) => t.plaintext ? (c(), u("textarea", {
      key: 0,
      class: "inpt-root inpt-plain",
      value: t.modelValue,
      placeholder: t.placeholder,
      disabled: t.disabled,
      readonly: t.readonly,
      style: k(p.value),
      rows: "4",
      onInput: x[0] || (x[0] = (S) => y.$emit("update:modelValue", S.target.value))
    }, null, 44, Ae)) : (c(), u("input", {
      key: 1,
      class: "inpt-root",
      type: t.type,
      value: t.modelValue,
      placeholder: t.placeholder,
      disabled: t.disabled,
      readonly: t.readonly,
      style: k(p.value),
      onInput: x[1] || (x[1] = (S) => y.$emit("update:modelValue", t.type === "number" ? S.target.value === "" ? null : Number(S.target.value) : S.target.value)),
      onKeydown: ve(r, ["enter"])
    }, null, 44, He));
  }
}, qe = /* @__PURE__ */ U(Ye, [["__scopeId", "data-v-d65dc90b"]]), We = { class: "ccp-root" }, Ke = { class: "ccp-swatch-hex" }, Ge = { class: "ccp-dialog" }, Je = { class: "ccp-footer" }, Qe = {
  __name: "Ctrl_ColorPicker",
  props: {
    modelValue: { type: String, default: "#ffffff" },
    color: { type: String, default: "" },
    teleport: { type: String, default: "body" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: m }) {
    de((h) => ({
      ad6fc5a2: r.value
    }));
    const e = t, g = m, p = f(() => e.teleport !== "body"), r = f(() => e.color || "var(--rizui_themecolor)"), y = R(!1), x = R("#ffffff"), S = R(null), V = R(null), I = R(0), B = R(0), w = R(100), E = R(0);
    let L = !1, _ = !1;
    const z = f(() => ({ background: `linear-gradient(to right, #fff, ${$(I.value, 100, 50)})` })), F = f(() => ({
      background: r.value,
      borderColor: r.value
    }));
    function $(h, i, d) {
      i /= 100, d /= 100;
      const M = i * Math.min(d, 1 - d), P = (H) => {
        const W = (H + h / 30) % 12;
        return Math.round(255 * (d - M * Math.max(Math.min(W - 3, 9 - W, 1), -1))).toString(16).padStart(2, "0");
      };
      return `#${P(0)}${P(8)}${P(4)}`;
    }
    function a(h) {
      let i = 0, d = 0, M = 0;
      h = h.replace("#", ""), h.length === 3 ? (i = parseInt(h[0] + h[0], 16) / 255, d = parseInt(h[1] + h[1], 16) / 255, M = parseInt(h[2] + h[2], 16) / 255) : h.length >= 6 && (i = parseInt(h.slice(0, 2), 16) / 255, d = parseInt(h.slice(2, 4), 16) / 255, M = parseInt(h.slice(4, 6), 16) / 255);
      const P = Math.max(i, d, M), H = Math.min(i, d, M), W = (P + H) / 2;
      let q = 0, Q = 0;
      if (P !== H) {
        const te = P - H;
        Q = W > 0.5 ? te / (2 - P - H) : te / (P + H), P === i ? q = ((d - M) / te + (d < M ? 6 : 0)) / 6 : P === d ? q = ((M - i) / te + 2) / 6 : q = ((i - d) / te + 4) / 6;
      }
      return q = Math.round(q * 360), q < 0 && (q += 360), { h: q, s: Math.round(Q * 100), l: Math.round(W * 100) };
    }
    function l(h) {
      const i = a(h);
      I.value = i.h, B.value = i.h / 360 * 100, w.value = i.s;
      const d = 100 - i.s * 0.5;
      E.value = d > 0 ? Math.max(0, Math.min(100, Math.round(100 - i.l * 100 / d))) : 0;
    }
    function s() {
      const h = w.value, i = 100 - h * 0.5, d = Math.round(i * (100 - E.value) / 100);
      x.value = $(I.value, h, d);
    }
    function n() {
      x.value = e.modelValue || "#ffffff", l(x.value), y.value = !0;
    }
    function b() {
      g("update:modelValue", x.value), y.value = !1;
    }
    function v() {
      y.value = !1;
    }
    function X(h) {
      const i = String(h ?? "");
      x.value = i, /^#[0-9a-fA-F]{6}$/.test(i.trim()) && l(i.trim());
    }
    function N(h) {
      const i = V.value.getBoundingClientRect(), d = Math.max(0, Math.min(100, (h - i.left) / i.width * 100));
      B.value = d, I.value = Math.round(d / 100 * 360), s();
    }
    function K(h) {
      L = !0, N(h.clientX || h.touches[0].clientX);
    }
    function J(h, i) {
      const d = S.value.getBoundingClientRect();
      w.value = Math.max(0, Math.min(100, (h - d.left) / d.width * 100)), E.value = Math.max(0, Math.min(100, (i - d.top) / d.height * 100)), s();
    }
    function D(h) {
      _ = !0, J(h.clientX || h.touches[0].clientX, h.clientY || h.touches[0].clientY);
    }
    function O(h) {
      L && N(h.clientX || h.touches[0].clientX), _ && J(h.clientX || h.touches[0].clientX, h.clientY || h.touches[0].clientY);
    }
    function Y() {
      L = !1, _ = !1;
    }
    return Z(() => {
      document.addEventListener("mousemove", O), document.addEventListener("mouseup", Y), document.addEventListener("touchmove", O, { passive: !1 }), document.addEventListener("touchend", Y);
    }), ee(() => {
      document.removeEventListener("mousemove", O), document.removeEventListener("mouseup", Y), document.removeEventListener("touchmove", O), document.removeEventListener("touchend", Y);
    }), (h, i) => (c(), u("div", We, [
      o("div", {
        class: "ccp-swatch",
        onClick: n
      }, [
        o("span", {
          class: "ccp-swatch-dot",
          style: k({ background: t.modelValue || "#ffffff" })
        }, null, 4),
        o("span", Ke, j(t.modelValue || "#ffffff"), 1)
      ]),
      (c(), se(ue, { to: t.teleport }, [
        ne(ie, { name: "ccp-dialog" }, {
          default: le(() => [
            y.value ? (c(), u("div", {
              key: 0,
              class: T(["ccp-overlay", { "ccp-anchored": p.value }]),
              onClick: A(v, ["self"])
            }, [
              o("div", Ge, [
                o("div", { class: "ccp-header" }, [
                  i[1] || (i[1] = o("span", null, "选取颜色", -1)),
                  o("button", {
                    class: "ccp-close",
                    onClick: v,
                    type: "button"
                  }, [...i[0] || (i[0] = [
                    o("svg", {
                      viewBox: "0 0 24 24",
                      width: "12",
                      height: "12",
                      stroke: "currentColor",
                      "stroke-width": "2.5",
                      fill: "none"
                    }, [
                      o("path", { d: "M18 6L6 18M6 6l12 12" })
                    ], -1)
                  ])])
                ]),
                o("div", {
                  class: "ccp-sv",
                  ref_key: "svRef",
                  ref: S,
                  style: k(z.value),
                  onMousedown: D,
                  onTouchstart: A(D, ["prevent"])
                }, [
                  o("div", {
                    class: "ccp-sv-thumb",
                    style: k({ left: w.value + "%", top: E.value + "%" })
                  }, null, 4)
                ], 36),
                o("div", {
                  class: "ccp-hue",
                  ref_key: "hueRef",
                  ref: V,
                  onMousedown: K,
                  onTouchstart: A(K, ["prevent"])
                }, [
                  o("div", {
                    class: "ccp-hue-thumb",
                    style: k({ left: B.value + "%" })
                  }, null, 4)
                ], 544),
                o("div", Je, [
                  o("span", {
                    class: "ccp-preview",
                    style: k({ background: x.value })
                  }, null, 4),
                  ne(qe, {
                    "model-value": x.value,
                    color: e.color,
                    placeholder: "#ffffff",
                    style: { flex: "1", height: "28px", "text-align": "left", "font-family": "inherit" },
                    "onUpdate:modelValue": X
                  }, null, 8, ["model-value", "color"]),
                  o("button", {
                    class: "ccp-confirm",
                    style: k(F.value),
                    onClick: b,
                    type: "button",
                    title: "确定"
                  }, [...i[2] || (i[2] = [
                    o("svg", {
                      viewBox: "0 0 24 24",
                      width: "16",
                      height: "16",
                      stroke: "currentColor",
                      "stroke-width": "3",
                      fill: "none"
                    }, [
                      o("path", { d: "M20 6L9 17l-5-5" })
                    ], -1)
                  ])], 4)
                ])
              ])
            ], 2)) : C("", !0)
          ]),
          _: 1
        })
      ], 8, ["to"]))
    ]));
  }
}, cn = /* @__PURE__ */ U(Qe, [["__scopeId", "data-v-d77bbea9"]]), Ze = { class: "ctl-bubble" }, et = {
  class: "ctl-arrow",
  viewBox: "0 0 12 6",
  width: "12",
  height: "6"
}, tt = ["fill"], nt = { class: "ctl-labels" }, lt = {
  __name: "Ctrl_Timeline",
  props: {
    modelValue: { type: Number, default: 0 },
    min: { type: Number, default: 0 },
    max: { type: Number, default: 120 },
    step: { type: Number, default: 1 },
    color: { type: String, default: "" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: m }) {
    const e = t, g = m, p = R(null), r = R(!1), y = f(() => e.color || "var(--rizui_themecolor)"), x = f(() => e.max - e.min <= 60 ? 2 : 3), S = f(() => {
      const n = [], b = e.max - e.min <= 60 ? 5 : 10;
      for (let v = e.min; v <= e.max; v += b) n.push(v);
      return n;
    }), V = f(() => {
      const n = [], b = e.max - e.min <= 60 ? 15 : 30;
      for (let v = e.min; v <= e.max; v += b) n.push(v);
      return n;
    });
    function I(n) {
      const b = e.max - e.min;
      return b <= 0 ? 0 : (n - e.min) / b * 100;
    }
    function B(n) {
      const b = Math.floor(n / 60), v = n % 60;
      return `${b}:${String(v).padStart(2, "0")}`;
    }
    const w = f(() => {
      const n = e.max - e.min;
      return n <= 0 ? 0 : Math.max(0, Math.min(100, (e.modelValue - e.min) / n * 100));
    }), E = f(() => ({
      "--ctl-color": y.value
    })), L = f(() => ({
      width: w.value + "%",
      background: `color-mix(in srgb, ${y.value} 20%, transparent)`
    })), _ = f(() => ({
      left: w.value + "%"
    })), z = f(() => {
      const n = Math.max(0, Math.round(e.modelValue || 0));
      return `${Math.floor(n / 60)}:${String(n % 60).padStart(2, "0")}`;
    });
    function F(n) {
      return e.step < 1 ? parseFloat(n) : parseInt(n);
    }
    function $(n) {
      if (!p.value) return e.modelValue;
      const b = p.value.getBoundingClientRect(), v = Math.max(0, Math.min(1, (n - b.left) / b.width)), X = e.min + v * (e.max - e.min), N = Math.round(X / e.step) * e.step;
      return F(Math.max(e.min, Math.min(e.max, N)).toFixed(10));
    }
    function a(n) {
      var v, X;
      r.value = !0;
      const b = $(n.clientX || ((X = (v = n.touches) == null ? void 0 : v[0]) == null ? void 0 : X.clientX) || 0);
      b !== e.modelValue && g("update:modelValue", b);
    }
    function l(n) {
      var v, X;
      if (!r.value) return;
      const b = $(n.clientX || ((X = (v = n.touches) == null ? void 0 : v[0]) == null ? void 0 : X.clientX) || 0);
      b !== e.modelValue && g("update:modelValue", b);
    }
    function s() {
      r.value = !1;
    }
    return Z(() => {
      document.addEventListener("mousemove", l), document.addEventListener("mouseup", s), document.addEventListener("touchmove", l, { passive: !1 }), document.addEventListener("touchend", s);
    }), ee(() => {
      document.removeEventListener("mousemove", l), document.removeEventListener("mouseup", s), document.removeEventListener("touchmove", l), document.removeEventListener("touchend", s);
    }), (n, b) => (c(), u("div", {
      class: "ctl-root",
      style: k(E.value)
    }, [
      o("div", {
        ref_key: "trackRef",
        ref: p,
        class: T(["ctl-track", { "ctl-dragging": r.value }]),
        onMousedown: A(a, ["left"]),
        onTouchstart: A(a, ["prevent"])
      }, [
        o("div", {
          class: "ctl-fill",
          style: k(L.value)
        }, null, 4),
        (c(!0), u(oe, null, ae(S.value, (v) => (c(), u("div", {
          key: "t" + v,
          class: T(["ctl-tick", { "ctl-tick-minor": v % x.value !== 0 }]),
          style: k({ left: I(v) + "%" })
        }, null, 6))), 128)),
        o("div", {
          class: "ctl-indicator",
          style: k(_.value)
        }, [
          o("span", Ze, j(z.value), 1),
          (c(), u("svg", et, [
            o("path", {
              d: "M0 0l6 6 6-6z",
              fill: y.value
            }, null, 8, tt)
          ]))
        ], 4)
      ], 34),
      o("div", nt, [
        (c(!0), u(oe, null, ae(V.value, (v) => (c(), u("span", {
          key: "l" + v,
          class: "ctl-label-tick",
          style: k({ left: I(v) + "%" })
        }, j(B(v)), 5))), 128))
      ])
    ], 4));
  }
}, un = /* @__PURE__ */ U(lt, [["__scopeId", "data-v-7d2a0003"]]), ot = {
  __name: "Ctrl_Button",
  props: {
    text: { type: String, default: "" },
    textInstyle: { type: Object, default: () => ({}) },
    lIcon: { type: String, default: "" },
    lIconInstyle: { type: Object, default: () => ({}) },
    rIcon: { type: String, default: "" },
    rIconInstyle: { type: Object, default: () => ({}) },
    primary: { type: Boolean, default: !1 },
    danger: { type: Boolean, default: !1 },
    long: { type: Boolean, default: !1 },
    color: { type: String, default: "" },
    disabled: { type: Boolean, default: !1 },
    type: { type: String, default: "button" },
    href: { type: String, default: "" },
    target: { type: String, default: "_blank" }
  },
  emits: ["click"],
  setup(t) {
    const m = t, e = f(() => ({
      "--cbtn-color": m.color || "var(--rizui_themecolor)"
    }));
    return (g, p) => (c(), se(pe(t.href ? "a" : "button"), {
      class: T(["cbtn-root", {
        "cbtn-primary": t.primary,
        "cbtn-danger": t.danger,
        "cbtn-long": t.long,
        "cbtn-disabled": t.disabled
      }]),
      style: k(e.value),
      type: t.href ? void 0 : t.type,
      href: t.href || void 0,
      target: t.href ? t.target : void 0,
      rel: t.href ? "noopener noreferrer" : void 0,
      disabled: t.href ? void 0 : t.disabled || void 0,
      onClick: p[0] || (p[0] = (r) => g.$emit("click", r))
    }, {
      default: le(() => [
        t.lIcon ? (c(), u("i", {
          key: 0,
          class: T(["cbtn-icon", t.lIcon]),
          style: k(t.lIconInstyle)
        }, null, 6)) : C("", !0),
        t.text ? (c(), u("span", {
          key: 1,
          class: "cbtn-text",
          style: k(t.textInstyle)
        }, j(t.text), 5)) : C("", !0),
        t.rIcon ? (c(), u("i", {
          key: 2,
          class: T(["cbtn-icon", t.rIcon]),
          style: k(t.rIconInstyle)
        }, null, 6)) : C("", !0)
      ]),
      _: 1
    }, 8, ["class", "style", "type", "href", "target", "rel", "disabled"]));
  }
}, rn = /* @__PURE__ */ U(ot, [["__scopeId", "data-v-f2eb13ae"]]), at = {
  key: 0,
  class: "imd-toolbar"
}, st = { class: "imd-toolbar-btns" }, it = ["title", "disabled", "onClick"], ct = { class: "imd-tabs" }, ut = { class: "imd-body" }, rt = ["value", "placeholder", "disabled", "onKeydown"], dt = ["innerHTML"], ft = {
  __name: "Inpt_mdeditor",
  props: {
    modelValue: { type: String, default: "" },
    placeholder: { type: String, default: "在此输入 Markdown 内容…" },
    color: { type: String, default: "" },
    disabled: { type: Boolean, default: !1 },
    readonly: { type: Boolean, default: !1 },
    height: { type: String, default: "320px" },
    default: { type: String, default: "edit" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: m }) {
    const e = t, g = m, p = R(null), r = R(e.default === "preview" ? "preview" : "edit"), y = f(() => ({
      "--imd-color": e.color || "var(--rizui_themecolor)"
    })), x = [
      { id: "bold", title: "加粗", icon: "fa-solid fa-bold" },
      { id: "italic", title: "斜体", icon: "fa-solid fa-italic" },
      { id: "strike", title: "删除线", icon: "fa-solid fa-strikethrough" },
      { id: "h2", title: "二级标题", icon: "fa-solid fa-heading" },
      { id: "quote", title: "引用", icon: "fa-solid fa-quote-left" },
      { id: "ul", title: "无序列表", icon: "fa-solid fa-list-ul" },
      { id: "ol", title: "有序列表", icon: "fa-solid fa-list-ol" },
      { id: "task", title: "任务列表", icon: "fa-solid fa-list-check" },
      { id: "code", title: "行内代码", icon: "fa-solid fa-code" },
      { id: "codeblock", title: "代码块", icon: "fa-solid fa-code-branch" },
      { id: "link", title: "链接", icon: "fa-solid fa-link" },
      { id: "image", title: "图片", icon: "fa-solid fa-image" },
      { id: "table", title: "表格", icon: "fa-solid fa-table" },
      { id: "hr", title: "分割线", icon: "fa-solid fa-minus" }
    ];
    function S($, a = "", l = "") {
      const s = p.value;
      if (!s) return;
      const n = s.selectionStart, b = s.selectionEnd, v = e.modelValue.slice(n, b) || l, X = e.modelValue.slice(0, n) + $ + v + a + e.modelValue.slice(b);
      g("update:modelValue", X), re(() => {
        s.focus();
        const N = n + $.length + v.length;
        s.setSelectionRange(N, N);
      });
    }
    function V($) {
      const a = p.value;
      if (!a) return;
      const l = a.selectionStart, s = e.modelValue.lastIndexOf(`
`, l - 1) + 1, n = e.modelValue.indexOf(`
`, l), b = n === -1 ? e.modelValue.length : n, v = e.modelValue.slice(s, b), X = e.modelValue.slice(0, s) + $ + v + e.modelValue.slice(b);
      g("update:modelValue", X), re(() => {
        a.focus();
        const N = s + $.length + (l - s);
        a.setSelectionRange(N, N);
      });
    }
    function I() {
      const $ = p.value;
      if (!$) return;
      const a = $.selectionStart, l = $.selectionEnd, s = e.modelValue.slice(0, a) + "  " + e.modelValue.slice(l);
      g("update:modelValue", s), re(() => {
        $.focus(), $.setSelectionRange(a + 2, a + 2);
      });
    }
    function B($) {
      r.value !== "edit" ? (r.value = "edit", re(() => w($))) : w($);
    }
    function w($) {
      if (!e.disabled)
        switch ($) {
          case "bold":
            S("**", "**", "加粗文本");
            break;
          case "italic":
            S("*", "*", "斜体文本");
            break;
          case "strike":
            S("~~", "~~", "删除文本");
            break;
          case "h2":
            V("## ");
            break;
          case "quote":
            V("> ");
            break;
          case "ul":
            V("- ");
            break;
          case "ol":
            V("1. ");
            break;
          case "task":
            V("- [ ] ");
            break;
          case "code":
            S("`", "`", "code");
            break;
          case "codeblock":
            S("\n```\n", "\n```\n", "const a = 1");
            break;
          case "link":
            S("[", "](https://)", "链接文字");
            break;
          case "image":
            S("![", "](https://example.com/image.png)", "图片描述");
            break;
          case "table":
            S(`
| 列1 | 列2 |
| --- | --- |
| 内容 | 内容 |
`, "", "");
            break;
          case "hr":
            S(`
---
`, "", "");
            break;
        }
    }
    const E = f(() => F(e.modelValue || ""));
    function L($) {
      return $.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;");
    }
    function _($) {
      return /^(https?:|mailto:|#|\/|\.\/|\.\.\/|data:image\/)/i.test($) ? $ : "#";
    }
    function z($) {
      let a = L($);
      return a = a.replace(/`([^`]+)`/g, "<code>$1</code>"), a = a.replace(/!\[([^\]]*)\]\(([^)\s]+)\)/g, (l, s, n) => `<img src="${_(n)}" alt="${s}" loading="lazy">`), a = a.replace(/\[([^\]]+)\]\(([^)\s]+)\)/g, (l, s, n) => `<a href="${_(n)}" target="_blank" rel="noopener">${s}</a>`), a = a.replace(/\*\*([^*]+)\*\*/g, "<strong>$1</strong>"), a = a.replace(/(^|[^*])\*([^*\s][^*]*)\*/g, "$1<em>$2</em>"), a = a.replace(/~~([^~]+)~~/g, "<del>$1</del>"), a;
    }
    function F($) {
      const l = $.replace(/\r\n/g, `
`).split(`
`), s = [];
      let n = 0, b = [];
      const v = () => {
        b.length && (s.push("<p>" + b.map(z).join("<br>") + "</p>"), b = []);
      };
      for (; n < l.length; ) {
        const N = l[n].trim();
        if (!N) {
          v(), n++;
          continue;
        }
        if (N.startsWith("```")) {
          v();
          const O = [];
          for (n++; n < l.length && !l[n].trim().startsWith("```"); )
            O.push(l[n]), n++;
          n++, s.push("<pre><code>" + L(O.join(`
`)) + "</code></pre>");
          continue;
        }
        const K = N.match(/^(#{1,6})\s+(.*)$/);
        if (K) {
          v();
          const O = K[1].length;
          s.push(`<h${O}>${z(K[2])}</h${O}>`), n++;
          continue;
        }
        if (/^(-{3,}|\*{3,}|_{3,})$/.test(N)) {
          v(), s.push("<hr>"), n++;
          continue;
        }
        if (N.includes("|") && l[n + 1] && /^\s*\|?[\s:\-|]+\|?\s*$/.test(l[n + 1].trim())) {
          v();
          const O = (d) => d.replace(/^\|/, "").replace(/\|$/, "").split("|").map((M) => M.trim()), Y = O(N);
          n += 2;
          const h = [];
          for (; n < l.length && l[n].trim() && l[n].includes("|"); )
            h.push(O(l[n])), n++;
          let i = "<table><thead><tr>" + Y.map((d) => "<th>" + z(d) + "</th>").join("") + "</tr></thead><tbody>";
          i += h.map((d) => "<tr>" + d.map((M) => "<td>" + z(M) + "</td>").join("") + "</tr>").join(""), i += "</tbody></table>", s.push(i);
          continue;
        }
        if (N.startsWith(">")) {
          v();
          const O = [];
          for (; n < l.length && l[n].trim().startsWith(">"); )
            O.push(l[n].trim().replace(/^>\s?/, "")), n++;
          s.push("<blockquote><p>" + O.map(z).join("<br>") + "</p></blockquote>");
          continue;
        }
        if (N.match(/^[-*+]\s+(.*)$/)) {
          for (v(), s.push("<ul>"); n < l.length; ) {
            const O = l[n].trim().match(/^[-*+]\s+(.*)$/);
            if (!O) break;
            const Y = O[1].match(/^\[([ xX])\]\s+(.*)$/);
            if (Y) {
              const h = Y[1].toLowerCase() === "x" ? " checked" : "";
              s.push(`<li class="imd-task${h}"><input type="checkbox"${h} disabled> ${z(Y[2])}</li>`);
            } else
              s.push("<li>" + z(O[1]) + "</li>");
            n++;
          }
          s.push("</ul>");
          continue;
        }
        if (N.match(/^\d+\.\s+(.*)$/)) {
          for (v(), s.push("<ol>"); n < l.length; ) {
            const O = l[n].trim().match(/^\d+\.\s+(.*)$/);
            if (!O) break;
            s.push("<li>" + z(O[1]) + "</li>"), n++;
          }
          s.push("</ol>");
          continue;
        }
        b.push(N), n++;
      }
      return v(), s.join(`
`);
    }
    return ($, a) => (c(), u("div", {
      class: T(["imd-root", { "imd-readonly": t.readonly }]),
      style: k(y.value)
    }, [
      t.readonly ? C("", !0) : (c(), u("div", at, [
        o("div", st, [
          (c(), u(oe, null, ae(x, (l) => o("button", {
            key: l.id,
            class: "imd-tb",
            title: l.title,
            type: "button",
            disabled: t.disabled,
            onMousedown: a[0] || (a[0] = A(() => {
            }, ["prevent"])),
            onClick: (s) => B(l.id)
          }, [
            o("i", {
              class: T(l.icon)
            }, null, 2)
          ], 40, it)), 64))
        ]),
        o("div", ct, [
          o("button", {
            class: T(["imd-tab", { "imd-tab-active": r.value === "edit" }]),
            type: "button",
            onMousedown: a[1] || (a[1] = A(() => {
            }, ["prevent"])),
            onClick: a[2] || (a[2] = (l) => r.value = "edit")
          }, [...a[6] || (a[6] = [
            o("i", { class: "fa-solid fa-pen" }, null, -1),
            fe(" 编辑", -1)
          ])], 34),
          o("button", {
            class: T(["imd-tab", { "imd-tab-active": r.value === "preview" }]),
            type: "button",
            onMousedown: a[3] || (a[3] = A(() => {
            }, ["prevent"])),
            onClick: a[4] || (a[4] = (l) => r.value = "preview")
          }, [...a[7] || (a[7] = [
            o("i", { class: "fa-solid fa-eye" }, null, -1),
            fe(" 预览", -1)
          ])], 34)
        ])
      ])),
      o("div", ut, [
        !t.readonly && r.value === "edit" ? (c(), u("textarea", {
          key: 0,
          ref_key: "taRef",
          ref: p,
          class: "imd-editor",
          value: t.modelValue,
          placeholder: t.placeholder,
          disabled: t.disabled,
          style: k({ height: t.height }),
          onInput: a[5] || (a[5] = (l) => $.$emit("update:modelValue", l.target.value)),
          onKeydown: ve(A(I, ["prevent"]), ["tab"])
        }, null, 44, rt)) : C("", !0),
        t.readonly || r.value === "preview" ? (c(), u("div", {
          key: 1,
          class: "imd-preview",
          style: k({ height: t.height })
        }, [
          o("div", {
            class: "imd-preview-inner",
            innerHTML: E.value
          }, null, 8, dt)
        ], 4)) : C("", !0)
      ])
    ], 6));
  }
}, dn = /* @__PURE__ */ U(ft, [["__scopeId", "data-v-b8a33009"]]), mt = { class: "ui-card" }, vt = { class: "ui-card-title" }, ht = {
  key: 0,
  class: "ui-card-desc"
}, pt = {
  __name: "UiCard",
  props: {
    title: { type: String, required: !0 },
    desc: { type: String, default: "" }
  },
  setup(t) {
    return (m, e) => (c(), u("div", mt, [
      o("div", vt, j(t.title), 1),
      t.desc ? (c(), u("div", ht, j(t.desc), 1)) : C("", !0),
      G(m.$slots, "default", {}, void 0, !0)
    ]));
  }
}, fn = /* @__PURE__ */ U(pt, [["__scopeId", "data-v-b67dfbf3"]]), yt = { class: "nro-overlay" }, gt = { class: "nro-badge" }, bt = {
  __name: "NotReadyOverlay",
  props: {
    text: { type: String, default: "🚧 等待实现" }
  },
  setup(t) {
    return (m, e) => (c(), u("div", yt, [
      o("span", gt, j(t.text), 1)
    ]));
  }
}, kt = /* @__PURE__ */ U(bt, [["__scopeId", "data-v-b2c4028f"]]), $t = { class: "ucr-label-col" }, xt = {
  key: 0,
  class: "ucr-hint"
}, wt = {
  key: 1,
  class: "ucr-slot-wrap"
}, St = {
  __name: "UiConfigRow",
  props: {
    label: { type: String, required: !0 },
    hint: { type: String, default: "" },
    stack: { type: Boolean, default: !1 },
    notReady: { type: Boolean, default: !1 },
    notReadyText: { type: String, default: "等待实现" }
  },
  setup(t) {
    return (m, e) => (c(), u("div", {
      class: T(["ucr-row", { "ucr-not-ready": t.notReady, "ucr-stack": t.stack }])
    }, [
      o("div", $t, [
        o("label", null, j(t.label), 1),
        t.hint ? (c(), u("span", xt, j(t.hint), 1)) : C("", !0),
        t.stack ? (c(), u("div", wt, [
          G(m.$slots, "default", {}, void 0, !0)
        ])) : C("", !0)
      ]),
      t.stack ? C("", !0) : G(m.$slots, "default", {}, void 0, !0, 0),
      t.notReady ? (c(), se(kt, {
        key: 1,
        text: t.notReadyText
      }, null, 8, ["text"])) : C("", !0)
    ], 2));
  }
}, mn = /* @__PURE__ */ U(St, [["__scopeId", "data-v-4e6d2936"]]), Vt = ["checked", "disabled"], Mt = {
  __name: "UiSwitch",
  props: {
    modelValue: { type: Boolean, default: !1 },
    color: { type: String, default: "" },
    disabled: { type: Boolean, default: !1 }
  },
  emits: ["update:modelValue"],
  setup(t) {
    const m = t, e = f(() => m.color || "var(--rizui_themecolor)"), g = f(() => ({
      background: m.modelValue ? e.value : void 0
    }));
    return (p, r) => (c(), u("label", {
      class: T(["uis-root", { "uis-disabled": t.disabled }])
    }, [
      o("input", {
        type: "checkbox",
        checked: t.modelValue,
        disabled: t.disabled,
        onChange: r[0] || (r[0] = (y) => p.$emit("update:modelValue", y.target.checked))
      }, null, 40, Vt),
      o("span", {
        class: "uis-track",
        style: k(g.value)
      }, null, 4)
    ], 2));
  }
}, vn = /* @__PURE__ */ U(Mt, [["__scopeId", "data-v-eb90b6df"]]), Ct = { class: "uist-value" }, It = 400, Et = 60, Lt = {
  __name: "UiSlider",
  props: {
    modelValue: { type: Number, default: 0 },
    min: { type: Number, default: 0 },
    max: { type: Number, default: 100 },
    step: { type: Number, default: 1 },
    color: { type: String, default: "" },
    display: { type: String, default: "" },
    displayValue: { type: String, default: "" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: m }) {
    const e = t, g = m, p = R(null), r = f(() => e.color || "var(--rizui_themecolor)"), y = f(() => {
      const a = e.max - e.min;
      return a <= 0 ? 0 : Math.max(0, Math.min(100, (e.modelValue - e.min) / a * 100));
    }), x = f(() => ({
      background: `linear-gradient(to right, color-mix(in srgb, ${r.value} 20%, transparent) ${y.value}%, transparent ${y.value}%)`,
      borderColor: r.value
    }));
    function S(a) {
      return e.step < 1 ? parseFloat(a) : parseInt(a);
    }
    function V(a) {
      const l = e.modelValue + a * e.step, s = Math.max(e.min, Math.min(e.max, l)), n = S(s.toFixed(10));
      n !== e.modelValue && g("update:modelValue", n);
    }
    const I = R(!1);
    function B(a) {
      if (!p.value) return e.modelValue;
      const l = p.value.getBoundingClientRect(), s = Math.max(0, Math.min(1, (a - l.left) / l.width)), n = e.min + s * (e.max - e.min), b = Math.round(n / e.step) * e.step;
      return S(Math.max(e.min, Math.min(e.max, b)).toFixed(10));
    }
    function w(a) {
      var b, v;
      I.value = !0;
      const l = a.target, s = l.closest(".uist-minus"), n = l.closest(".uist-plus");
      if (s)
        V(-1), z(-1);
      else if (n)
        V(1), z(1);
      else {
        const X = B(a.clientX || ((v = (b = a.touches) == null ? void 0 : b[0]) == null ? void 0 : v.clientX) || 0);
        X !== e.modelValue && g("update:modelValue", X);
      }
    }
    function E(a) {
      var s, n;
      if (!I.value) return;
      F();
      const l = B(a.clientX || ((n = (s = a.touches) == null ? void 0 : s[0]) == null ? void 0 : n.clientX) || 0);
      l !== e.modelValue && g("update:modelValue", l);
    }
    function L() {
      I.value = !1, F();
    }
    const _ = R(null);
    function z(a) {
      F(), _.value = setTimeout(() => {
        _.value = setInterval(() => V(a), Et);
      }, It);
    }
    function F() {
      _.value && (clearTimeout(_.value), clearInterval(_.value), _.value = null);
    }
    Z(() => {
      document.addEventListener("mousemove", E), document.addEventListener("mouseup", L), document.addEventListener("touchmove", E, { passive: !1 }), document.addEventListener("touchend", L);
    }), ee(() => {
      document.removeEventListener("mousemove", E), document.removeEventListener("mouseup", L), document.removeEventListener("touchmove", E), document.removeEventListener("touchend", L), F();
    });
    const $ = f(() => {
      if (e.displayValue) return e.displayValue;
      const a = e.step < 1 ? e.modelValue.toFixed(1) : e.modelValue;
      return e.display ? `${a}${e.display}` : String(a);
    });
    return (a, l) => (c(), u("div", {
      ref_key: "rootRef",
      ref: p,
      class: T(["uist-root", { "uist-dragging": I.value }]),
      style: k(x.value),
      onMousedown: A(w, ["left"]),
      onTouchstart: A(w, ["prevent"])
    }, [
      l[0] || (l[0] = o("button", { class: "uist-btn uist-minus" }, "−", -1)),
      o("span", Ct, j($.value), 1),
      l[1] || (l[1] = o("button", { class: "uist-btn uist-plus" }, "+", -1))
    ], 38));
  }
}, hn = /* @__PURE__ */ U(Lt, [["__scopeId", "data-v-0ec10baa"]]), Tt = { class: "uxt-root" }, Bt = {
  class: "uxt-arrow",
  viewBox: "0 0 12 6",
  width: "10",
  height: "5"
}, Rt = ["fill"], _t = { class: "uxt-label" }, Xt = { class: "uxt-labels" }, Ot = {
  __name: "UiTimeline",
  props: {
    modelValue: { type: Number, default: 0 },
    min: { type: Number, default: 0 },
    max: { type: Number, default: 120 },
    step: { type: Number, default: 1 },
    color: { type: String, default: "" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: m }) {
    const e = t, g = m, p = R(null), r = R(!1), y = f(() => e.color || "var(--rizui_themecolor)"), x = f(() => {
      const l = [], s = e.max - e.min <= 60 ? 10 : 30;
      for (let n = e.min; n <= e.max; n += s) l.push(n);
      return l;
    }), S = f(() => {
      const l = [], s = e.max - e.min <= 60 ? 15 : 30;
      for (let n = e.min; n <= e.max; n += s) l.push(n);
      return l;
    });
    function V(l) {
      const s = e.max - e.min;
      return s <= 0 ? 0 : (l - e.min) / s * 100;
    }
    function I(l) {
      const s = Math.floor(l / 60), n = l % 60;
      return `${s}:${String(n).padStart(2, "0")}`;
    }
    const B = f(() => {
      const l = e.max - e.min;
      return l <= 0 ? 0 : Math.max(0, Math.min(100, (e.modelValue - e.min) / l * 100));
    }), w = f(() => ({
      width: B.value + "%",
      background: `color-mix(in srgb, ${y.value} 20%, transparent)`
    })), E = f(() => ({
      left: B.value + "%"
    })), L = f(() => {
      const l = Math.max(0, Math.round(e.modelValue || 0));
      return `${Math.floor(l / 60)}:${String(l % 60).padStart(2, "0")}`;
    });
    function _(l) {
      return e.step < 1 ? parseFloat(l) : parseInt(l);
    }
    function z(l) {
      if (!p.value) return e.modelValue;
      const s = p.value.getBoundingClientRect(), n = Math.max(0, Math.min(1, (l - s.left) / s.width)), b = e.min + n * (e.max - e.min), v = Math.round(b / e.step) * e.step;
      return _(Math.max(e.min, Math.min(e.max, v)).toFixed(10));
    }
    function F(l) {
      var n, b;
      r.value = !0;
      const s = z(l.clientX || ((b = (n = l.touches) == null ? void 0 : n[0]) == null ? void 0 : b.clientX) || 0);
      s !== e.modelValue && g("update:modelValue", s);
    }
    function $(l) {
      var n, b;
      if (!r.value) return;
      const s = z(l.clientX || ((b = (n = l.touches) == null ? void 0 : n[0]) == null ? void 0 : b.clientX) || 0);
      s !== e.modelValue && g("update:modelValue", s);
    }
    function a() {
      r.value = !1;
    }
    return Z(() => {
      document.addEventListener("mousemove", $), document.addEventListener("mouseup", a), document.addEventListener("touchmove", $, { passive: !1 }), document.addEventListener("touchend", a);
    }), ee(() => {
      document.removeEventListener("mousemove", $), document.removeEventListener("mouseup", a), document.removeEventListener("touchmove", $), document.removeEventListener("touchend", a);
    }), (l, s) => (c(), u("div", Tt, [
      o("div", {
        ref_key: "trackRef",
        ref: p,
        class: "uxt-track",
        onMousedown: A(F, ["left"]),
        onTouchstart: A(F, ["prevent"])
      }, [
        o("div", {
          class: "uxt-fill",
          style: k(w.value)
        }, null, 4),
        (c(!0), u(oe, null, ae(x.value, (n) => (c(), u("div", {
          key: n,
          class: "uxt-tick",
          style: k({ left: V(n) + "%" })
        }, null, 4))), 128)),
        o("div", {
          class: "uxt-indicator",
          style: k(E.value)
        }, [
          (c(), u("svg", Bt, [
            o("path", {
              d: "M0 0l6 6 6-6z",
              fill: y.value
            }, null, 8, Rt)
          ])),
          o("span", _t, j(L.value), 1)
        ], 4)
      ], 544),
      o("div", Xt, [
        (c(!0), u(oe, null, ae(S.value, (n) => (c(), u("span", {
          key: n,
          class: "uxt-label-tick",
          style: k({ left: V(n) + "%" })
        }, j(I(n)), 5))), 128))
      ])
    ]));
  }
}, pn = /* @__PURE__ */ U(Ot, [["__scopeId", "data-v-fc0c2f4a"]]), Nt = {
  key: 0,
  class: "udd-drop"
}, Dt = ["onClick"], zt = {
  __name: "UiDropdown",
  props: {
    modelValue: { type: [String, Number], default: "" },
    options: { type: Array, default: () => [] },
    color: { type: String, default: "" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: m }) {
    de((w) => ({
      b64c2a56: y.value
    }));
    const e = t, g = m, p = R(null), r = R(!1), y = f(() => e.color || "var(--rizui_themecolor)"), x = f(() => {
      const w = e.options.find((E) => E.value === e.modelValue);
      return w ? w.label : String(e.modelValue || "");
    }), S = f(() => ({
      color: y.value,
      background: `color-mix(in srgb, ${y.value} 8%, transparent)`
    }));
    function V() {
      r.value = !r.value;
    }
    function I(w) {
      g("update:modelValue", w), r.value = !1;
    }
    function B(w) {
      p.value && !p.value.contains(w.target) && (r.value = !1);
    }
    return Z(() => {
      document.addEventListener("mousedown", B, !0), document.addEventListener("touchstart", B, !0);
    }), ee(() => {
      document.removeEventListener("mousedown", B, !0), document.removeEventListener("touchstart", B, !0);
    }), (w, E) => (c(), u("div", {
      ref_key: "rootRef",
      ref: p,
      class: T(["udd-root", { "udd-open": r.value }])
    }, [
      o("button", {
        class: "udd-trigger",
        onClick: V,
        type: "button"
      }, [
        o("span", null, j(x.value), 1),
        E[0] || (E[0] = o("svg", {
          class: "udd-arrow",
          viewBox: "0 0 12 7",
          width: "10",
          height: "6"
        }, [
          o("path", {
            d: "M1 1l5 5 5-5",
            stroke: "currentColor",
            "stroke-width": "1.5",
            fill: "none",
            "stroke-linecap": "round"
          })
        ], -1))
      ]),
      ne(ie, { name: "udd-pop" }, {
        default: le(() => [
          r.value ? (c(), u("div", Nt, [
            (c(!0), u(oe, null, ae(t.options, (L) => (c(), u("button", {
              key: L.value,
              class: T(["udd-opt", { "udd-active": L.value === t.modelValue }]),
              style: k(L.value === t.modelValue ? S.value : {}),
              onClick: (_) => I(L.value),
              type: "button"
            }, j(L.label), 15, Dt))), 128))
          ])) : C("", !0)
        ]),
        _: 1
      })
    ], 2));
  }
}, yn = /* @__PURE__ */ U(zt, [["__scopeId", "data-v-2f2af13d"]]), jt = { class: "ucp-root" }, Ft = { class: "ucp-swatch-hex" }, Ut = { class: "ucp-dialog" }, Pt = { class: "ucp-footer" }, At = ["value"], Ht = {
  __name: "UiColorPicker",
  props: {
    modelValue: { type: String, default: "#ffffff" },
    color: { type: String, default: "" },
    teleport: { type: String, default: "body" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: m }) {
    de((i) => ({
      v7fbf371c: r.value
    }));
    const e = t, g = m, p = f(() => e.teleport !== "body"), r = f(() => e.color || "var(--rizui_themecolor)"), y = R(!1), x = R("#ffffff"), S = R(null), V = R(null), I = R(0), B = R(0), w = R(100), E = R(0);
    let L = !1, _ = !1;
    const z = f(() => ({ background: `linear-gradient(to right, #fff, ${$(I.value, 100, 50)})` })), F = f(() => ({
      background: r.value,
      borderColor: r.value
    }));
    function $(i, d, M) {
      d /= 100, M /= 100;
      const P = d * Math.min(M, 1 - M), H = (W) => {
        const q = (W + i / 30) % 12;
        return Math.round(255 * (M - P * Math.max(Math.min(q - 3, 9 - q, 1), -1))).toString(16).padStart(2, "0");
      };
      return `#${H(0)}${H(8)}${H(4)}`;
    }
    function a(i) {
      let d = 0, M = 0, P = 0;
      i = i.replace("#", ""), i.length === 3 ? (d = parseInt(i[0] + i[0], 16) / 255, M = parseInt(i[1] + i[1], 16) / 255, P = parseInt(i[2] + i[2], 16) / 255) : i.length >= 6 && (d = parseInt(i.slice(0, 2), 16) / 255, M = parseInt(i.slice(2, 4), 16) / 255, P = parseInt(i.slice(4, 6), 16) / 255);
      const H = Math.max(d, M, P), W = Math.min(d, M, P), q = (H + W) / 2;
      let Q = 0, te = 0;
      if (H !== W) {
        const ce = H - W;
        te = q > 0.5 ? ce / (2 - H - W) : ce / (H + W), H === d ? Q = ((M - P) / ce + (M < P ? 6 : 0)) / 6 : H === M ? Q = ((P - d) / ce + 2) / 6 : Q = ((d - M) / ce + 4) / 6;
      }
      return Q = Math.round(Q * 360), Q < 0 && (Q += 360), { h: Q, s: Math.round(te * 100), l: Math.round(q * 100) };
    }
    function l(i) {
      const d = a(i);
      I.value = d.h, B.value = d.h / 360 * 100, w.value = d.s;
      const M = 100 - d.s * 0.5;
      E.value = M > 0 ? Math.max(0, Math.min(100, Math.round(100 - d.l * 100 / M))) : 0;
    }
    function s() {
      const i = w.value, d = 100 - i * 0.5, M = Math.round(d * (100 - E.value) / 100);
      x.value = $(I.value, i, M);
    }
    function n() {
      x.value = e.modelValue || "#ffffff", l(x.value), y.value = !0;
    }
    function b() {
      g("update:modelValue", x.value), y.value = !1;
    }
    function v() {
      y.value = !1;
    }
    function X(i) {
      const d = i.target.value.trim();
      x.value = d, /^#[0-9a-fA-F]{6}$/.test(d) && l(d);
    }
    function N() {
      /^#[0-9a-fA-F]{6}$/.test(x.value) && l(x.value);
    }
    function K(i) {
      const d = V.value.getBoundingClientRect(), M = Math.max(0, Math.min(100, (i - d.left) / d.width * 100));
      B.value = M, I.value = Math.round(M / 100 * 360), s();
    }
    function J(i) {
      L = !0, K(i.clientX || i.touches[0].clientX);
    }
    function D(i, d) {
      const M = S.value.getBoundingClientRect();
      w.value = Math.max(0, Math.min(100, (i - M.left) / M.width * 100)), E.value = Math.max(0, Math.min(100, (d - M.top) / M.height * 100)), s();
    }
    function O(i) {
      _ = !0, D(i.clientX || i.touches[0].clientX, i.clientY || i.touches[0].clientY);
    }
    function Y(i) {
      L && K(i.clientX || i.touches[0].clientX), _ && D(i.clientX || i.touches[0].clientX, i.clientY || i.touches[0].clientY);
    }
    function h() {
      L = !1, _ = !1;
    }
    return Z(() => {
      document.addEventListener("mousemove", Y), document.addEventListener("mouseup", h), document.addEventListener("touchmove", Y, { passive: !1 }), document.addEventListener("touchend", h);
    }), ee(() => {
      document.removeEventListener("mousemove", Y), document.removeEventListener("mouseup", h), document.removeEventListener("touchmove", Y), document.removeEventListener("touchend", h);
    }), (i, d) => (c(), u("div", jt, [
      o("div", {
        class: "ucp-swatch",
        onClick: n
      }, [
        o("span", {
          class: "ucp-swatch-dot",
          style: k({ background: t.modelValue || "#ffffff" })
        }, null, 4),
        o("span", Ft, j(t.modelValue || "#ffffff"), 1)
      ]),
      (c(), se(ue, { to: t.teleport }, [
        ne(ie, { name: "ucp-dialog" }, {
          default: le(() => [
            y.value ? (c(), u("div", {
              key: 0,
              class: T(["ucp-overlay", { "ucp-anchored": p.value }]),
              onClick: A(v, ["self"])
            }, [
              o("div", Ut, [
                o("div", { class: "ucp-header" }, [
                  d[0] || (d[0] = o("span", null, "选取颜色", -1)),
                  o("button", {
                    class: "ucp-close",
                    onClick: v
                  }, "✕")
                ]),
                o("div", {
                  class: "ucp-sv",
                  ref_key: "svRef",
                  ref: S,
                  style: k(z.value),
                  onMousedown: O,
                  onTouchstart: A(O, ["prevent"])
                }, [
                  o("div", {
                    class: "ucp-sv-thumb",
                    style: k({ left: w.value + "%", top: E.value + "%" })
                  }, null, 4)
                ], 36),
                o("div", {
                  class: "ucp-hue",
                  ref_key: "hueRef",
                  ref: V,
                  onMousedown: J,
                  onTouchstart: A(J, ["prevent"])
                }, [
                  o("div", {
                    class: "ucp-hue-thumb",
                    style: k({ left: B.value + "%" })
                  }, null, 4)
                ], 544),
                o("div", Pt, [
                  o("span", {
                    class: "ucp-preview",
                    style: k({ background: x.value })
                  }, null, 4),
                  o("input", {
                    type: "text",
                    class: "ucp-hex",
                    value: x.value,
                    onInput: X,
                    onBlur: N,
                    placeholder: "#ffffff"
                  }, null, 40, At),
                  o("button", {
                    class: "ucp-confirm",
                    style: k(F.value),
                    onClick: b
                  }, "确定", 4)
                ])
              ])
            ], 2)) : C("", !0)
          ]),
          _: 1
        })
      ], 8, ["to"]))
    ]));
  }
}, gn = /* @__PURE__ */ U(Ht, [["__scopeId", "data-v-d055fd73"]]), Yt = { class: "uid-header" }, qt = { class: "uid-title" }, Wt = { class: "uid-body" }, Kt = {
  key: 0,
  class: "uid-footer"
}, Gt = {
  __name: "UiDialog",
  props: {
    show: { type: Boolean, default: !1 },
    title: { type: String, default: "" },
    color: { type: String, default: "" },
    teleport: { type: String, default: "body" },
    width: { type: String, default: "auto" }
  },
  emits: ["update:show"],
  setup(t) {
    const m = t, e = f(() => m.teleport !== "body"), g = f(() => m.color || "var(--rizui_themecolor)"), p = f(() => ({
      width: m.width,
      "--uid-color": g.value
    }));
    return (r, y) => (c(), se(ue, { to: t.teleport }, [
      ne(ie, { name: "uid-pop" }, {
        default: le(() => [
          t.show ? (c(), u("div", {
            key: 0,
            class: T(["uid-overlay", { "uid-anchored": e.value }]),
            onClick: y[1] || (y[1] = A((x) => r.$emit("update:show", !1), ["self"]))
          }, [
            o("div", {
              class: "uid-box",
              style: k(p.value)
            }, [
              o("div", Yt, [
                o("span", qt, j(t.title), 1),
                o("button", {
                  class: "uid-close",
                  onClick: y[0] || (y[0] = (x) => r.$emit("update:show", !1))
                }, "✕")
              ]),
              o("div", Wt, [
                G(r.$slots, "default", {}, void 0, !0)
              ]),
              r.$slots.footer ? (c(), u("div", Kt, [
                G(r.$slots, "footer", {}, void 0, !0)
              ])) : C("", !0)
            ], 4)
          ], 2)) : C("", !0)
        ]),
        _: 3
      })
    ], 8, ["to"]));
  }
}, bn = /* @__PURE__ */ U(Gt, [["__scopeId", "data-v-9d712284"]]);
export {
  Zt as Cntr_Card,
  en as Cntr_Cfgrow,
  tn as Cntr_Infobox,
  Zt as Ct_Card,
  nn as Ctnr_Dialog,
  ln as Ctnr_Toast,
  rn as Ctrl_Button,
  cn as Ctrl_ColorPicker,
  sn as Ctrl_Dropdown,
  an as Ctrl_Slider,
  on as Ctrl_Switch,
  un as Ctrl_Timeline,
  qe as Inpt_Text,
  dn as Inpt_mdeditor,
  kt as NotReadyOverlay,
  fn as UiCard,
  gn as UiColorPicker,
  mn as UiConfigRow,
  bn as UiDialog,
  yn as UiDropdown,
  hn as UiSlider,
  vn as UiSwitch,
  pn as UiTimeline,
  Qt as openURL
};
