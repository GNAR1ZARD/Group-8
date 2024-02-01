package nl.tudelft.jpacman.level;

import nl.tudelft.jpacman.sprite.PacManSprites;
import org.junit.jupiter.api.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class PelletTest {

    private static final PacManSprites SPRITE_STORE = new PacManSprites();
    private Pellet ThePellet = new Pellet(1,SPRITE_STORE.getPelletSprite());
    @Test
    void testPelletConstructor(){
        assertThat(ThePellet.getValue()).isEqualTo(1);
        assertThat(ThePellet.getSprite()).isEqualTo(SPRITE_STORE.getPelletSprite());
    }
}
