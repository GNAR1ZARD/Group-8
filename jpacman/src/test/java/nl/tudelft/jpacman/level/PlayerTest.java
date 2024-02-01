package nl.tudelft.jpacman.level;

import org.junit.jupiter.api.Test;

import static org.assertj.core.api.Assertions.assertThat;
import nl.tudelft.jpacman.sprite.PacManSprites;
public class PlayerTest {
    /**
     * Do we get the correct value when testing whether
     * the player is alive?
     */
 //   assertThat((Player.isAlive()).isEqualTo(True));
 //   bool alive = Player.isAlive();
    private static final PacManSprites SPRITE_STORE = new PacManSprites();
    private PlayerFactory Factory = new PlayerFactory(SPRITE_STORE);
    private Player ThePlayer = Factory.createPacMan();

    @Test
    void testAlive(){
        assertThat(ThePlayer.isAlive()).isEqualTo(true);
    }
}
